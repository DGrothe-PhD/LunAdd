using System.Speech.Synthesis;

namespace LunAdd
{

    public partial class Element : StandardForm
    {
        readonly FieldType FieldType;
        bool CtrlIsDown = false;

        public Element(Form1 caller, FieldType fieldType)
        {
            InitializeComponent();
            FieldType = fieldType;
            lblTitle.Text = LocalFieldNames.GetText(fieldType);
            txtContent.Text = caller?.VCard?.GetEntry(fieldType.ToString())?.HelpReading() ?? "Leer";
            mute = false;
            txtContent.MouseWheel += OnMouseWheel;
            Focus();
        }

        private void OnMouseWheel(object? sender, MouseEventArgs e)
        {
            if (!CtrlIsDown) return;
            switch (e.Delta)
            {
                case > 0:
                    if (txtContent.Font.Size < 200)
                        txtContent.Font = new Font(txtContent.Font.Name, txtContent.Font.Size + 20);
                    break;
                default:
                    if (txtContent.Font.Size > 30)
                        txtContent.Font = new Font(txtContent.Font.Name, txtContent.Font.Size - 20);
                    break;
            }
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
            if (e.KeyCode == Keys.ControlKey)
            {
                CtrlIsDown = true;
                speaker.SpeakAsync("Steuerung gedrückt");
            }
        }

        private void Element_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                CtrlIsDown = false;
                speaker.SpeakAsync("Steuerung losgelassen");
            }
        }

        private void Element_Scroll(object sender, ScrollEventArgs e)
        {
            speaker.Speak("Scrollen");
        }

        private void txtContent_MouseEnter(object sender, EventArgs e)
        {
            txtContent.Focus();
        }
    }
}
