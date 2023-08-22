using System.Speech.Synthesis;

namespace LunAdd
{

    public partial class Element : StandardForm
    {
        SpeechSynthesizer speaker = new SpeechSynthesizer();
        Dictionary<FieldType, String> LocalFieldNames = UIFieldNames.GermanFieldNames;
        FieldType FieldType;
        bool CtrlIsDown = false;

        public Element(Form1 caller, FieldType fieldType)
        {
            InitializeComponent();
            FieldType = fieldType;
            lblTitle.Text = LocalFieldNames.GetText(fieldType);
            txtContent.Text = caller?.VCard?.GetEntry(fieldType.ToString())?.HelpReading() ?? "Leer";
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
