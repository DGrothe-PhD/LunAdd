

namespace LunAdd
{
    internal static class Extensions
    {
        internal static string GetText(this Dictionary<FieldType, String> enumdic, FieldType lookup)
        {
            if (enumdic.ContainsKey(lookup))
                return enumdic[lookup];
            else return string.Empty;
        }

        internal static string GetTextOrDefault(this Dictionary<FieldType, String> enumdic, string lookup)
        {
            if (Enum.TryParse(typeof(FieldType), lookup, out object? result) && result != null)
                return enumdic[(FieldType)result];
            else return lookup;
        }

        internal static string HelpReading(this string input)
        {
            string s = input;
            s = s.Replace("!!!", "‼️");
            s = s.Replace("!", "❗");
            s = s.Replace("?", "❓");
            return s;
        }

        internal static string HelpReadingNotes(this string input)
        {
            string[] snippets = input.Split("\n");
            var timemarkers = new[] {"Uhr", ":30", ":15", ":00", ":45", ".30", ".15", ".00", ".45" };
            for (int i=0; i<snippets.Length; i++)
            {
                var s = snippets[i].Trim();
                if (timemarkers.Any(s.Contains))
                {
                    //Need to check for key as a whole word. "Fr" -> "Freitag", whereas "Freundlich" stays.
                    foreach (string key in LocalUI.Weekdays.Keys)
                    {
                        int w = s.IndexOf(key);
                        if (w == -1) continue;
                        if (w + 2 < s.Length && !Char.IsLetter(s[w + 2]))
                            s = s.Replace("-", " bis ");
                            s = s.Replace(key, LocalUI.Weekdays[key]);
                    }
                    snippets[i] = s;
                }
            }
            return String.Join("\r\n", snippets);
        }

        internal static string HelpReadingPhone(this string input)
        {
            // far from elegant but at least it does it
            // otherwise it does seven-hundred dash thing or omits the dash
            var padding = input.ToCharArray()!.Select(x => " " + x);
            string s = HelpReading(String.Join("", padding) ?? "");
            s = s.Replace("-", "<break/> Strich <break/>");
            return s;
        }

        internal static string wrapSpeech(this String input)
        {
            string s = input;//.Replace("überholt", "<emphasis>überholt</emphasis>");
            String[] snippets = s.Split("\n");
            String modifiedInput = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" "
                + "xmlns:mstts=\"https://www.w3.org/2001/mstts\" xml:lang=\"de-DE \">"
                + String.Join("<break/>", snippets)
                + "</speak>";
            return modifiedInput;
        }
    }
    internal static class LocalUI
    {
        internal static Dictionary<String, String> Weekdays = new Dictionary<String, String>()
        {
            {"Mo", "Montag" }, {"Di", "Dienstag"}, {"Mi", "Mittwoch"}, {"Do", "Donnerstag"},
            {"Fr", "Freitag" }, {"Sa", "Samstag"}, {"So", "Sonntag"}
        };

        internal static Dictionary<FieldType, String> GermanFieldNames = new Dictionary<FieldType, String>()
        {
            {FieldType.WorkCountry, "Arbeitet in" },
            {FieldType.HomeCountry, "Land" },
            {FieldType.FullName, "Vollständiger Name" },
            {FieldType.FirstName, "Vorname" },
            {FieldType.LastName, "Zuname" },
            {FieldType.DisplayName, "Angezeigter Name" },
            {FieldType.HomeStreet, "Wohnhaft" }, {FieldType.HomeCity, "in" }, {FieldType.HomeZipCode, "Postleitzahl"},
            {FieldType.Company, "Firma" },
            {FieldType.PrimaryEmail, "Standard E-Mail" },
            {FieldType.WorkAddress, "Dienstliche Anschrift" },
            {FieldType.WorkZipCode, "in" },
            {FieldType.WorkCity, "Geschäftlich in" },
            {FieldType.BusinessEMail, "Geschäftliche E-Mail" },
            {FieldType.WorkPhone, "Dienstliche Telefonnummer" },
            {FieldType.HomePhone, "Telefon privat" },
            {FieldType.FaxNumber, "Faxnummer" },
            {FieldType.CellularNumber, "Handynummer" },
            {FieldType.OtherPhone, "Andere Telefonnummer" },
            {FieldType.Notes, "Bemerkungen" },
            {FieldType.WebPage1, "Webseite 1" },
            {FieldType.WebPage2, "Webseite 2" }
        };
    }
}
