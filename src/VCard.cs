using System.Text;

namespace LunAdd
{
    public class VCard
    {
        private Dictionary<string, string> AdressData;
        Dictionary<FieldType, String> LocalFieldNames = UIFieldNames.GermanFieldNames;
        // TODO multilanguage, centralized

        public VCard()
        {
            AdressData = new();
        }


        public void AppendLineToValue(string currentKey, string appendtext, bool protectLineBreaks = false)
        {
            AdressData[currentKey] += (protectLineBreaks ? "\\n" : "<br>")
                + Environment.NewLine + appendtext.Trim();
        }

        public void AddNewField(string field, string value)
        {
            if (AdressData.ContainsKey(field))
            {
                AddNewField(field + " (2)", value);
            }
            else
            {
                AdressData[field] = value;
            }
        }

        public void AppendFullName()
        {
            string[] fullName = new string[3];
            int namepieces = 0;
            foreach (var d in AdressData)
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
            AdressData["FullName"] = s;
        }

        string mlb = "<br>\r\n";
        /// <summary>
        /// Get full text of a VCard.
        /// </summary>
        /// <returns>VCard text (all fields)</returns>

        string searchableString;
        public override string ToString()
        {
            StringBuilder sb = new();
            if (AdressData.ContainsKey("FullName"))
                sb.AppendLine(AdressData["FullName"].HelpReading());
            foreach (var d in AdressData)
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
            if (AdressData.ContainsKey(v)) { return AdressData[v]; }
            return Lang.Resources.EmptyData;
        }

        internal static readonly List<String> excludedFields = new()
        { 
            "LastModifiedDate", "PhotoType", "PreferMailFormat", "AllowRemoteContent",
            "PopularityIndex", "PreferDisplayName", "UID"
        };
    }
}
