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
            catch (FileFormatException ex)
            {
                MessageBox.Show("Dateifehler. Details:" + Environment.NewLine + ex.Message ?? "");
                Close();
            }
            catch (Exception ex)
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
            UpdateFields(data?.cards[currentIndex - 1]);
        }
        private void UpdateFields(VCard? currentCard)
        {
            ignoreVCEvent = true;
            try
            {
                VCard = currentCard;
            }
            catch (Exception ex)
            {
                MessageBox.Show("CouldNotReadData".LookupTranslation());
                Close();
            }
            string entry = VCard?.ToString() ?? "";
            StringBuilder sb = new();

            sb.Append(entry);
            sb.Replace("\r\n\r\n", "<br>");
            entry = sb.ToString().Replace("<br>", Environment.NewLine);
            txtEntryInformation.Text = entry;
            txtSearchField.Text = currentIndex + " von " + data?.cards.Count.ToString() ?? "";
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

        private void BtnWorkPhone_Click(object sender, EventArgs e) => ElementInvoke(FieldType.WorkPhone);

        private void BtnHomePhone_Click(object sender, EventArgs e) => ElementInvoke(FieldType.HomePhone);

        private void BtnShowMobile_Click(object sender, EventArgs e) => ElementInvoke(FieldType.CellularNumber);

        private void BtnShowEMail_Click(object sender, EventArgs e) => ElementInvoke(FieldType.PrimaryEmail);

        private static bool disabledShortcuts = false;
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (disabledShortcuts) return;
            e.Handled = true;
            switch (e.KeyChar)
            {
                case '+':
                    FlipForward();
                    break;
                case '-':
                    FlipBack();
                    break;
                case ' ':
                    speaker.SpeakSsmlAsync(txtEntryInformation.Text.wrapSpeech());
                    break;
                case 'n':
                    ElementInvoke(FieldType.FullName);
                    break;
                case 'd':
                    ElementInvoke(FieldType.DisplayName);
                    break;
                case '@':
                    ElementInvoke(FieldType.PrimaryEmail);
                    break;
                case 't':
                    ElementInvoke(FieldType.HomePhone);
                    break;
                case 'f':
                    ElementInvoke(FieldType.FaxNumber);
                    break;
                case 'm':
                    ElementInvoke(FieldType.CellularNumber);
                    break;
                case 'w':
                    ElementInvoke(FieldType.WorkPhone);
                    break;
                case '#':
                    ElementInvoke(FieldType.Notes);
                    break;
                default:
                    break;
            }
        }

        private void btnOpening_Click(object sender, EventArgs e)
        {
            ElementInvoke(FieldType.Notes);
        }

        public virtual void Element_KeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                speaker.SpeakAsyncCancelAll();
                this.Close();
            }
            if(e.KeyCode == Keys.End) {
                e.SuppressKeyPress = true;
                disabledShortcuts = false;
                speaker.SpeakAsyncCancelAll();
            }
            if (e.KeyCode == Keys.F6)
            {
                disabledShortcuts = true;
                txtSearchField.Select();
                txtSearchField.Clear();
                return;
            }

            if( e.KeyCode == Keys.Right)
            {
                FlipForward();
            }
            if (e.KeyCode == Keys.Left)
            {
                FlipBack();
            }

            if (e.KeyCode == Keys.Enter && disabledShortcuts)
            {
                disabledShortcuts = false;
                string searchtext = txtSearchField.Text;
                VCard? finding = data?.cards?.FirstOrDefault(
                    x => x.ToString().Contains(txtSearchField.Text)
                );
                if (finding != null)
                {
                    UpdateFields(finding);
                    speaker.SpeakAsync("FoundOne".LookupTranslation());
                }
                else
                {
                    MessageBox.Show(
                        String.Format("NothingFound".LookupTranslation(), searchtext)
                    );
                }
            }
        }
    }
}