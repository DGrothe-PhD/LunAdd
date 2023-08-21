using System.Text;

namespace LunAdd
{
    public partial class Form1 : StandardForm
    {
        private int currentIndex = 1;
        private bool ignoreVCEvent = false;
        public VCard? VCard { get; private set; }
        private Engine? data;
        public Form1()
        {
            try
            {
                InitializeComponent();
                btnBack.BackgroundImage = Image.FromFile("Resources/ArrowBack.png");
                btnBack.BackgroundImageLayout = ImageLayout.Stretch;
                btnForward.BackgroundImage = Image.FromFile("Resources/ArrowForward.png");
                btnForward.BackgroundImageLayout = ImageLayout.Stretch;
                Run();
                numIndex.Maximum = data?.cards?.Count ?? 1;
                numIndex.Minimum = 1;
            }
            catch(FileFormatException ex)
            {
                MessageBox.Show("Dateiformatfehler. Details:" + Environment.NewLine + ex.Message ?? "");
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace?.ToString() ?? "");
                Close();
            }
        }

        private void Run()
        {
            data = new();
            UpdateFields();
        }

        private void UpdateFields()
        {
            ignoreVCEvent = true;
            try
            {
                VCard = data?.cards[currentIndex - 1];
            }
            catch(Exception ex)
            {
                MessageBox.Show("Problem beim Lesen der Datenquelle.");
                Close();
            }
            string entry = VCard?.ToString() ?? "";
            StringBuilder sb = new();
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
            FlipForward();
        }

        private void FlipForward()
        {
            if (currentIndex < data?.cards.Count)
            {
                currentIndex++;
                UpdateFields();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FlipBack();
        }

        private void FlipBack()
        {
            if (currentIndex > 1)
            {
                currentIndex--;
                UpdateFields();
            }
        }

        private void numIndex_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreVCEvent) return;
            currentIndex = (int)numIndex.Value;
            UpdateFields();
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
            if(e.KeyChar == '+')
            {
                e.Handled = true;
                FlipForward();
            }
            else if (e.KeyChar == '-')
            {
                e.Handled = true;
                FlipBack();
            }
            //
            if (e.KeyChar == ' ')
            {
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

    }
}