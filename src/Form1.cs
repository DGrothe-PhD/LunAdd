using System.Speech.Synthesis;
using System.Text;

namespace LunAdd
{
    public partial class Form1 : StandardForm
    {
        private readonly SpeechSynthesizer speaker = new SpeechSynthesizer();
        private int currentIndex = 1;
        private bool ignoreVCEvent = false;
        public VCard? VCard { get; private set; }
        private Engine? data;
        public Form1()
        {
            InitializeComponent();
            btnBack.BackgroundImage = Image.FromFile("Resources/ArrowBack.png");
            btnBack.BackgroundImageLayout = ImageLayout.Stretch;
            btnForward.BackgroundImage = Image.FromFile("Resources/ArrowForward.png");
            btnForward.BackgroundImageLayout = ImageLayout.Stretch;
            run();
            numIndex.Maximum = data?.cards?.Count ?? 1;
            numIndex.Minimum = 1;
        }

        private void run()
        {
            data = new();
            updateFields();
        }

        private void updateFields()
        {
            ignoreVCEvent = true;
            VCard = data?.cards[currentIndex - 1];
            string entry = VCard?.ToString() ?? "";
            StringBuilder sb = new StringBuilder();
            entry.Split("\r\n").ToList().ForEach(x => sb.Append(x.Trim()));
            entry = sb.ToString().Replace("\\n", Environment.NewLine);
            txtEntryInformation.Text = entry;
            txtSuchstring.Text = currentIndex + " von " + data?.cards.Count.ToString() ?? "";
            numIndex.Value = currentIndex;

            btnForward.Visible = (currentIndex < data?.cards.Count);
            btnBack.Visible = (currentIndex > 1);
            ignoreVCEvent = false;
        }


        private void btnForward_Click(object sender, EventArgs e)
        {
            if (currentIndex < data?.cards.Count)
            {
                currentIndex++;
                updateFields();
                //if (!btnBack.Visible) btnBack.Visible = true;
            }
            //if (currentIndex == data?.cards.Count)
            //{
            //    btnForward.Visible = false;
            //}
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentIndex > 1)
            {
                currentIndex--;
                updateFields();
                //if (!btnForward.Visible) btnForward.Visible = true;
            }
            //if (currentIndex == 1)
            //{
            //    btnBack.Visible = false;
            //}
        }
        private void numIndex_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreVCEvent) return;
            currentIndex = (int)numIndex.Value;
            updateFields();
        }

        private void ElementInvoke(FieldType fieldtype)
        {
            Element element = new(this, fieldtype);
            element.Show();
        }

        private void btnWorkPhone_Click(object sender, EventArgs e) => ElementInvoke(FieldType.WorkPhone);

        private void btnHomePhone_Click(object sender, EventArgs e) => ElementInvoke(FieldType.HomePhone);

        private void btnShowMobile_Click(object sender, EventArgs e) => ElementInvoke(FieldType.CellularNumber);

        private void btnShowEMail_Click(object sender, EventArgs e) => ElementInvoke(FieldType.PrimaryEmail);

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                /*
                 * speaker.Rate = Convert.ToInt32(speedUpDown.Value);
                 * speaker.Volume = Convert.ToInt32(volumeUpDown.Value);
                 * */
                //speaker.SpeakAsync(txtEntryInformation.Text);
                speaker.SpeakSsmlAsync(txtEntryInformation.Text.wrapSpeech());
            }
            if (e.KeyChar == 'n')
            {
                e.Handled = true;
                ElementInvoke(FieldType.FullName);
            }
            else if (e.KeyChar == 'd')
            {
                e.Handled = true;
                ElementInvoke(FieldType.DisplayName);
            }
            else if (e.KeyChar == '@')
            {
                e.Handled = true;
                ElementInvoke(FieldType.PrimaryEmail);
            }
            else if (e.KeyChar == 't')
            {
                e.Handled = true;
                ElementInvoke(FieldType.HomePhone);
            }
            else if (e.KeyChar == 'f')
            {
                e.Handled = true;
                ElementInvoke(FieldType.FaxNumber);
            }
            else if (e.KeyChar == 'm')
            {
                e.Handled = true;
                ElementInvoke(FieldType.CellularNumber);
            }
            else if (e.KeyChar == 'w')
            {
                e.Handled = true;
                ElementInvoke(FieldType.WorkPhone);
            }
            else if (e.KeyChar == '#')
            {
                e.Handled = true;
                ElementInvoke(FieldType.Notes);
            }
        }

        private void btnOpening_Click(object sender, EventArgs e)
        {
            ElementInvoke(FieldType.Notes);
        }

        /*
         * TODO
         * 1-2 Comboboxen für Auswahl von Adressfeldern
         * Piktogrammbuttons für:
         *  - Zeige Adresse (vergrößert in neuem Textfenster)
         *  - Zeige Mailadresse (vergrößert in neuem Textfenster)
         *  - Zeige Notizen (vergrößert in neuem Textfenster)
         *  - Zeige Telefonnummer (optional: Einsprechen der Ziffern)
         *  
         *  Nutzerführung mit Shortcut-Tasten!!!
         *  - Ins Suchfeld springen
         *  - Suche abschicken (Eingabe)
         *  - Zeige Telefonnummer und so (von da oben das alles)
         */
    }
}