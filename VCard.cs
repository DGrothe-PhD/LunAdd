using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunAdd
{
    public class VCard
    {
        private Dictionary<string, string> Adressdaten;
        Dictionary<FieldType, String> LocalFieldNames = LocalUI.GermanFieldNames;
        // TODO multilanguage, centralized

        public VCard()
        {
            Adressdaten = new();
        }

        public void AppendLineToValue(string currentKey, string appendtext)
        {
            Adressdaten[currentKey] += Environment.NewLine + appendtext;
        }

        public void AddNewField(string field, string value)
        {
            if(Adressdaten.ContainsKey(field))
            {
                AddNewField("#"+field + "1", value);
                var message = this.ToString();
                message = (message.Length > 150) ? message[0..150] : message;
                MessageBox.Show($"Corrupt CSV file. Double entry {field} found for card:\r\n" 
                    + message + "..."
                );
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

        public override string ToString()
        {
            StringBuilder sb = new();
            if(Adressdaten.ContainsKey("FullName"))
                sb.Append(Adressdaten["FullName"] + "\r\n");
            foreach (var d in Adressdaten)
            {
                if (excludedFields.Contains(d.Key)) continue;
                if (d.Key == "FullName") continue;
                sb.Append($"{LocalFieldNames.GetTextOrDefault(d.Key)}: {d.Value.HelpReading()}\r\n");
            }
            return sb.ToString();
        }

        internal string GetEntry(string v)
        {
            if (Adressdaten.ContainsKey(v)) { return Adressdaten[v]; }
            return "Leer";
        }

        internal static readonly List<String> excludedFields = new()
        { "LastModifiedDate", "PhotoType", "PreferMailFormat", "AllowRemoteContent",
            "PopularityIndex", "PreferDisplayName"};
    }
}
