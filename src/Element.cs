using System.Speech.Synthesis;

namespace LunAdd
{

    public partial class Element : StandardForm
    {
        readonly FieldType FieldType;

        public Element(Form1 caller, FieldType fieldType)
        {
            InitializeComponent();
            FieldType = fieldType;
            lblTitle.Text = LocalFieldNames.GetText(fieldType);
            txtContent.Text = caller?.VCard?.GetEntry(fieldType.ToString())?.HelpReading() ?? "Leer";
            mute = false;
            this.Focus();
        }

        private string PreparePrompt()
        {
            if (FieldType.ToString().Contains("Phone") || FieldType.ToString().Contains("Number"))
            {
                return txtContent.Text.HelpReadingPhone().wrapSpeech();
            }

            if (FieldType.ToString().Contains("Notes"))
            {
                return txtContent.Text.HelpReadingNotes().wrapSpeech();
            }

            return txtContent.Text.wrapSpeech();
        }


        private void Element_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                if (!mute) speaker.SpeakAsync(lblTitle.Text);
                SayThis = new Prompt(PreparePrompt(), SynthesisTextFormat.Ssml);
                if(!mute) speaker.SpeakAsync(SayThis);
            }
        }

        private void Element_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                mute = true;
                var current = speaker.GetCurrentlySpokenPrompt();

                if (current != null)
                {
                    speaker.SpeakAsyncCancelAll();
                }

                this.Close();
            }
        }
    }
}
