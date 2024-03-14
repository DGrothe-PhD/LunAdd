using System.Text;
using System.Text.RegularExpressions;

namespace LunAdd
{
    public class VCard
    {
        private Dictionary<string, string> Adressdaten;
        Dictionary<FieldType, String> LocalFieldNames = UIFieldNames.GermanFieldNames;
        // TODO multilanguage, centralized

        public VCard()
        {
            Adressdaten = new();
        }


        public void AppendLineToValue(string currentKey, string appendtext, bool protectLineBreaks = false)
        {
            Adressdaten[currentKey] += (protectLineBreaks?"\\n":"<br>") 
                + Environment.NewLine + appendtext.Trim();
        }

        int numOfDoubleEntries = 0;
        public void AddNewField(string field, string value)
        {
            if (Adressdaten.ContainsKey(field))
            {
                AddNewField(field + " (2)", value);
            }
            else
            {
                Adressdaten[field] = value;
            }
        }

        public void AppendFullName()
        {
            string[] fullName = new string[3];
            int namepieces = 0;
            foreach (var d in Adressdaten)
            {
                if (namepieces < 3)
                {
                    if (excludedFields.Contains(d.Key)) continue;
                    if (d.Key.StartsWith("JobTitle")) { fullName[0] = d.Value; namepieces++; }
                    else if (d.Key == "FirstName") { fullName[1] = d.Value; namepieces++; }
                    else if (d.Key == "LastName") { fullName[2] = d.Value; namepieces++; }
                }
                else break;
            }
            string s = String.Join(" ", fullName.ToList().Where(x => !String.IsNullOrEmpty(x)).ToList());
            Adressdaten["FullName"] = s;
        }

        string mlb = "<br>\r\n";
        /// <summary>
        /// Get full text of a VCard.
        /// </summary>
        /// <returns>VCard text (all fields)</returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            if (Adressdaten.ContainsKey("FullName"))
                sb.AppendLine(Adressdaten["FullName"].HelpReading());
            foreach (var d in Adressdaten)
            {
                if (excludedFields.Contains(d.Key)) continue;
                if (d.Key == "FullName") continue;

                if (UIFieldNames.VCardFieldNames.TryGetValue(d.Key, out FieldType lookupfield))
                {
                    if (LocalFieldNames.TryGetValue(lookupfield, out string? fieldname))
                    {
                        //sb.Append($"{fieldname}: {d.Value}"+ mlb);
                        sb.Append($"{fieldname}: {d.Value.HelpReading()}");
                        continue;
                    }
                }
                sb.AppendLine($"{LocalFieldNames.GetTextOrDefault(d.Key)}: {d.Value.HelpReading()}");
            }
            return sb.ToString();
        }

        internal string GetEntry(string v)
        {
            if (Adressdaten.ContainsKey(v)) { return Adressdaten[v]; }
            return Lang.Resources.EmptyData;
        }

        internal static readonly List<String> excludedFields = new()
        { "LastModifiedDate", "PhotoType", "PreferMailFormat", "AllowRemoteContent",
            "PopularityIndex", "PreferDisplayName", "UID"};
    }
}
