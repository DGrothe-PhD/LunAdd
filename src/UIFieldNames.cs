﻿

using System.Diagnostics;
using System.Text.RegularExpressions;
using Lang;

namespace LunAdd
{
    internal static class Extensions
    {
        internal static string LookupTranslation(this string s)
        {
            try
            {
                return Lang.Resources.ResourceManager.GetString(s) ?? s;
            }
            catch(Exception)
            {
                return s;
            }
        }

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

        /// <summary>
        /// <paramref name="comp"/> example: StringComparison.OrdinalIgnoreCase
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp">Comparison (e.g. case-insensitive)</param>
        /// <returns>True if <paramref name="source"/> contains <paramref name="toCheck"/></returns>
        internal static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        internal static string HelpReading(this string input)
        {
            string s = input;
            s = s.Replace("<br>", Environment.NewLine);
            s = s.Replace("!!!", "‼️");
            s = s.Replace("!", "❗");
            s = s.Replace("?", "❓");
            return s;
        }

        internal static string HelpReadingNotes(this string input)
        {
            //string[] snippets = input.Split("<br>");
            string[] snippets = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var timemarkers = new[] { "Uhr", ":30", ":15", ":00", ":45", ".30", ".15", ".00", ".45", "n.V.", "n. V." };
            for (int i = 0; i < snippets.Length; i++)
            {
                var s = snippets[i].Trim();
                if (timemarkers.Any(s.Contains))
                {
                    foreach (string key in UIFieldNames.Weekdays.Keys)
                    {
                        var match = $"({key})([.\\s])*[-]";
                        s = Regex.Replace(s, match, UIFieldNames.Weekdays[key] + " bis ");
                        match = $"({key})([.\\s])";
                        s = Regex.Replace(s, match, UIFieldNames.Weekdays[key] + " ");
                    }
                    s = Regex.Replace(s, @"n[.]\s*V[.]", "nach Vereinbarung");
                    snippets[i] = s;
                }
            }
            var result = String.Join("\r\n", snippets);
            if (Debugger.IsAttached)
                MessageBox.Show(result);
            return result;
        }

        internal static string HelpReadingPhone(this string input)
        {
            if (input.Contains(Lang.Resources.EmptyData))
                return input;

            string s = input;
            var prefixT = "<say-as interpret-as=\"telephone\">";
            var suffix = "</say-as>";
            s = s.Replace("-", suffix + "<break/> Strich <break/>" + prefixT);
            s = prefixT + s + suffix;
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
    internal static class UIFieldNames
    {
        private static string languageCode = "de-DE";
        public static string Language { get => languageCode; }

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
            {FieldType.NameList, "Name" },
            {FieldType.DisplayName, "Angezeigter Name" },
            {FieldType.HomeStreet, "Wohnhaft" }, {FieldType.HomeCity, "in" }, {FieldType.HomeZipCode, "Postleitzahl"},
            {FieldType.AdressList, "Anschrift" },
            {FieldType.PrimaryEmail, "Standard E-Mail" },
            {FieldType.SecondEmail, "Zweite E-Mail" },
            {FieldType.OtherEmail, "Andere E-Mail" },
            {FieldType.Company, "Firma" },
            {FieldType.Title, "Titel" },  {FieldType.Position, "Position" },
            {FieldType.PrimaryBusinessEMail, "Geschäftliche Haupt-E-Mail" },
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

        internal static Dictionary<String, FieldType> VCardFieldNames = new()
        {
            { "N", FieldType.NameList },
            { "ADR", FieldType.AdressList },
            { "FN", FieldType.FullName},
            { "EMAIL;PREF=1", FieldType.PrimaryEmail },
            { "EMAIL;TYPE=work", FieldType.BusinessEMail },
            { "EMAIL;PREF=1;TYPE=work", FieldType.PrimaryBusinessEMail },
            { "EMAIL", FieldType.OtherEmail},
            { "EMAIL;TYPE=other", FieldType.OtherEmail},
            { "TEL;VALUE=TEXT", FieldType.HomePhone},
            { "TEL;TYPE=work;VALUE=TEXT", FieldType.WorkPhone},
            { "TEL;TYPE=fax;VALUE=TEXT", FieldType.FaxNumber },
            { "TEL;TYPE=cell;VALUE=TEXT", FieldType.CellularNumber},
            { "NOTE", FieldType.Notes },
            { "TITLE", FieldType.Title },
            { "POSITION", FieldType.Position },
        };
    }
}
