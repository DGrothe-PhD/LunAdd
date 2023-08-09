namespace LunAdd
{
    public class Engine
    {
        //static CultureInfo invC = CultureInfo.InvariantCulture;
        //static CultureInfo accessLocal = new CultureInfo("de-DE");
        StreamReader? sr;
        VCard? currentCard;
        string currentGUI = "", currentField = "";
        public string CurrentGUI {get => currentGUI; private set { currentGUI = value;} }

        private readonly string datei1 = "Resources/Mappe1.csv";

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
                    if (daten.Length < 3)
                    {
                        currentCard?.AppendLineToValue(currentField, zeile);
                        continue;
                    }

                    //gui present for next field.
                    string gui = daten[0];

                    if (gui.Length < 2) continue;
                    // Next card arrived so put this to the list first.
                    if (!gui.Equals(currentGUI))
                    {
                        currentGUI = gui;
                        if (currentCard != null)
                        {
                            currentCard.AppendFullName();
                            cards.Add(currentCard);
                        }
                        currentCard = new();
                    }

                    // home country telephone number lost its beginning "0"
                    if ((daten[1].Contains("Phone") || daten[1].Contains("Number"))) 
                    {
                        if (daten[2].StartsWith("+49 "))
                            daten[2] = "0" + daten[2][4..];
                        else if (daten[2].StartsWith("+"))
                            daten[2] = "Auslandsnummer";
                        else if (daten[2][0] != '0')
                            daten[2] = "0" + daten[2];
                    }
                    currentField = daten[1];
                    try
                    {
                        currentCard?.AddNewField(daten[1], daten[2]);
                    }
                    catch(Exception e)
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
    }
}
