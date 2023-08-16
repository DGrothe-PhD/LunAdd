namespace LunAdd
{
    public class Engine
    {
        //static CultureInfo invC = CultureInfo.InvariantCulture;
        //static CultureInfo accessLocal = new CultureInfo("de-DE");
        StreamReader? sr;
        VCard? currentCard;
        string currentGUI = "", currentField = "", currentVCardField = "";
        public string CurrentGUI { get => currentGUI; private set { currentGUI = value; } }

        private readonly string datei1 = "Resources/Mappe1.csv";
        //private readonly string datei1 = "Resources/privat_firmen.csv";
        //private readonly string datei1 = "Resources/test.csv";

        public List<VCard> cards;

        public Engine()
        {
            currentGUI = string.Empty;
            cards = new List<VCard>();
            FileRead();//TODO customize filename via combobox.
        }

        private void FileRead()
        {
            try
            {
                FileInfo info = new(datei1);
                Console.WriteLine("Dateigröße: " + info.Length + "\n");

                FileStream fs = new(datei1, FileMode.Open);
                sr = new StreamReader(fs);

                while (sr.Peek() != -1)
                {
                    string zeile = sr.ReadLine() ?? "";
                    zeile = zeile.TrimEnd('"');
                    if (zeile.Count(f => f == '"') == 1)
                        zeile += '"';
                    if (zeile.Length < 3) continue;

                    string[] daten = zeile.TrimEnd(';').Split(';');

                    //Notes can be multiliners so append them to the preceding dictionary value.
                    // VCard format is multiline too, but special. Therein, the colon is the major column separator!
                    if (currentField.Contains("vCard"))
                    {
                        //VCard mode is on
                        ReadVCardDetails(zeile);
                        continue;
                    }

                    if (currentField != "" && daten.Length < 3)
                    {
                        if (!char.IsLetterOrDigit(zeile[0]) && zeile.Count(f => f == zeile[0]) == zeile.Length)
                        {
                            //prevent the engine from speaking "======" verbosely
                            currentCard?.AppendLineToValue(currentField, "(Querlinie)");
                            continue;
                        }
                        currentCard?.AppendLineToValue(currentField, zeile);
                        continue;
                    }

                    //gui present for next field.
                    string gui_candidate = daten[0];

                    if (gui_candidate.Length < 2) continue;

                    // Next card arrived so put this to the list first.
                    if (gui_candidate.Length == 36 && !gui_candidate.Equals(currentGUI))
                    {
                        currentGUI = gui_candidate;
                        if (currentCard != null)
                        {
                            currentCard.AppendFullName();
                            cards.Add(currentCard);
                        }
                        currentCard = new();
                    }

                    // home country telephone number lost its beginning "0"
                    if (daten[1].Contains("Phone") || daten[1].Contains("Number"))
                    {
                        if (daten[2].StartsWith("+49 "))
                            daten[2] = "0" + daten[2][4..];
                        else if (daten[2].StartsWith("+"))
                            daten[2] = "Auslandsnummer";
                        else if (daten[2][0] != '0')
                            daten[2] = "0" + daten[2];
                    }
                    currentField = daten[1];//my way of dealing with multiline fields.
                    try
                    {
                        currentCard?.AddNewField(daten[1], daten[2]);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }


                    // Dry run first
                    //if (anzahl > 15) break;
                }
                // Put last one 
                if (currentCard != null)
                {
                    currentCard.AppendFullName();
                    cards.Add(currentCard);
                }
            }
            catch (Exception ex)
            {
                sr?.Close();
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }

        int trynumtimes = 0;
        private void ReadVCardDetails(string zeile)
        {
            if (zeile.StartsWith("END:VCARD"))
            {
                currentField = "ENDOFCARD";//VCard mode switched off
                return;
            }
            string[] daten = zeile.Split(':', 2);
            try
            {
                //TODO adapt to conventional field names, BUT take care of double fields that way
                //Take care of the "\n"'s.
                //There can be extra colons in the text for notes.
                if (currentVCardField != "" && daten.Length == 1)
                {
                    if (!char.IsLetterOrDigit(zeile[0]) && zeile.Count(f => f == zeile[0]) == zeile.Length)
                    {
                        //prevent the engine from speaking "======" verbosely
                        currentCard?.AppendLineToValue(currentVCardField, "(Querlinie)");
                        return;
                    }
                    currentCard?.AppendLineToValue(currentVCardField, zeile);
                    return;
                }
                currentVCardField = daten[0];
                currentCard?.AddNewField(daten[0], daten[1]);
            }
            catch (Exception e)
            {
                if (trynumtimes < 3)
                    MessageBox.Show(e.ToString() + " in \n" + zeile);
                trynumtimes++;
            }
        }
    }
}
