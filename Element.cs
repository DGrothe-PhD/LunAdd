using System.Speech.Synthesis;

namespace LunAdd
{
    public enum FieldType
    {
        FullName,
        FirstName,
        LastName,
        DisplayName,
        //
        HomeStreet,
        HomeCity,
        HomeZipCode,
        HomeCountry,
        Company,
        PrimaryEmail,
        //
        WorkAddress,
        WorkZipCode,
        WorkCity,
        WorkCountry,
        BusinessEMail,
        //
        WorkPhone,
        HomePhone,
        FaxNumber,
        CellularNumber,
        OtherPhone,
        //
        Notes,
        WebPage1,
        WebPage2
    }

    public partial class Element : StandardForm
    {
        SpeechSynthesizer speaker = new SpeechSynthesizer();
        Dictionary<FieldType, String> LocalFieldNames = LocalUI.GermanFieldNames;
        FieldType FieldType;

        public Element(Form1 caller, FieldType fieldType)
        {
            InitializeComponent();
            this.FieldType = fieldType;
            lblTitle.Text = LocalFieldNames.GetText(fieldType);
            txtContent.Text = caller?.VCard?.GetEntry(fieldType.ToString())?.HelpReading() ?? "Leer";
            this.Focus();
        }

        private void Element_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                /*
                 * speaker.Rate = Convert.ToInt32(speedUpDown.Value);
                 * speaker.Volume = Convert.ToInt32(volumeUpDown.Value);
                 * */
                speaker.SpeakAsync(lblTitle.Text);
                if (FieldType.ToString().Contains("Phone") || FieldType.ToString().Contains("Number"))
                {
                    speaker.SpeakSsmlAsync((txtContent.Text.HelpReadingPhone()).wrapSpeech());
                }
                else if (FieldType.ToString().Contains("Notes"))
                {
                    speaker.SpeakSsmlAsync((txtContent.Text.HelpReadingNotes()).wrapSpeech());
                }
                else
                    speaker.SpeakAsync(txtContent.Text);
            }
        }

        private void Element_KeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                this.Close();
            }
        }
    }
}
