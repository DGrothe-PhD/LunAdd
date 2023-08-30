namespace LunAdd
{
    public class Engine
    {
        //static CultureInfo invC = CultureInfo.InvariantCulture;
        //static CultureInfo accessLocal = new CultureInfo("de-DE");
        StreamReader? sr;
        FileStream? stream;
        VCard? currentCard;
        string currentGUI = "", currentField = "", currentVCardField = "";
        public string CurrentGUI { get => currentGUI; private set { currentGUI = value; } }

        private readonly string datei1 = "Resources/Mappe1.csv";
        //private readonly string datei1 = "Resources/privat_firmen.csv";
        private readonly string datei2 = "Resources/test.csv";

        public List<VCard> cards;

        public Engine()
        {
            currentGUI = string.Empty;
            cards = new List<VCard>();
            FileRead(datei1);//TODO customize filename via combobox.
            FileRead(datei2);
        }

        private void FileRead(string filename)
        {
            try
            {
                FileInfo info = new(filename);
                Console.WriteLine("Dateigröße: " + info.Length + "\n");

                stream = new(filename, FileMode.Open);
                sr = new StreamReader(stream);

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
                            currentCard?.AppendLineToValue(currentField, "(Querlinie)", false);
                            continue;
                        }
                        currentCard?.AppendLineToValue(currentField, zeile, false);
                        continue;
                    }

                    //gui present for next field.
                    string gui_candidate = daten[0];

                    if (gui_candidate.Length < 2) continue;//ignore worthless lines

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
                            daten[2] = "Auslandsnummer " + daten[2];
                        else if (daten[2][0] != '0')
                            daten[2] = "0" + daten[2];
                    }
                    currentField = daten[1];//my way of dealing with multiline fields.

                    //ignore vcard markers.
                    daten[2] = daten[2].Replace("BEGIN:VCARD", "");
                    if (String.IsNullOrEmpty(daten[2].Trim('"')))
                        continue;

                    currentCard?.AddNewField(daten[1], daten[2]);

                }
                // Put last one 
                if (currentCard != null)
                {
                    currentCard.AppendFullName();
                    cards.Add(currentCard);
                }

                sr?.Close();
                stream?.Close();
            }
            catch (Exception ex)
            {
                sr?.Close();
                stream?.Close();
                throw new FileFormatException(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        int trynumtimes = 0;

        private void ReadVCardDetails(string textline)
        {
            if (textline.StartsWith("END:VCARD"))
            {
                currentField = "";//VCard mode switched off
                return;
            }
            string[] daten = textline.Split(':', 2);
            if (daten[0].Contains("VERSION"))
                return;

            try
            {
                //There can be extra colons in the text for notes.
                if ( currentVCardField != "" && daten.Length == 1 )
                {
                    ExtractMultiLiner(textline);
                    return;
                }
                // Take legacy address field's name if possible, else take fieldname as in the database.
                if (UIFieldNames.VCardFieldNames.TryGetValue(daten[0], out FieldType lookupfield))
                {
                    currentVCardField = lookupfield.ToString();
                }
                else if (currentVCardField.StartsWith("Notes"))
                {
                    //If you're here, CSV carries ':' in notes that don't indicate field names.
                    ExtractMultiLiner(textline);
                    return;
                }
                else 
                {
                    currentVCardField = daten[0];
                }
                currentCard?.AddNewField(currentVCardField, daten[1]);
            }
            catch (Exception e)
            {
                if (trynumtimes < 3)
                    MessageBox.Show(e.ToString() + " in \n" + textline);
                trynumtimes++;
            }
        }

        private void ExtractMultiLiner(string textline)
        {
            if (!char.IsLetterOrDigit(textline[0]) && textline.Count(f => f == textline[0]) == textline.Length)
            {
                //prevent the engine from speaking "======" verbosely
                currentCard?.AppendLineToValue(currentVCardField, "(Querlinie)");
                return;
            }
            currentCard?.AppendLineToValue(currentVCardField, textline.Trim());
            return;
        }
    }
}
