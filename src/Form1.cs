using System.Text;
using System.Windows.Forms;

namespace LunAdd
{
    public partial class Form1 : StandardForm
    {
        private int currentIndex = 1;
        private bool ignoreVCEvent = false;
        public VCard? VCard { get; private set; }
        private Engine? data;
        private List<VCard>? findings;
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
        private int hopPosition;

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
                if (disabledShortcuts)
                {
                    //ESC - leaves the search field and accepts shortcuts.
                    disabledShortcuts = false;
                    return;
                }

                speaker.SpeakAsyncCancelAll();
                this.Close();
            }
            if (e.KeyCode == Keys.End && !disabledShortcuts)
            {
                e.SuppressKeyPress = true;
                disabledShortcuts = false;
                speaker.SpeakAsyncCancelAll();
            }
            if (e.KeyCode == Keys.F6)
            {
                // Full-text search: hit F6 and type some text to search for.
                disabledShortcuts = true;
                txtSearchField.Select();
                txtSearchField.Clear();
                return;
            }

            // while typing search text don't flip card entries but allow cursor to move
            if (e.KeyCode == Keys.Right && !disabledShortcuts )
            {
                FlipForward();
            }
            if (e.KeyCode == Keys.Left && !disabledShortcuts )
            {
                FlipBack();
            }

            if (e.KeyCode == Keys.Enter && disabledShortcuts)
            {
                disabledShortcuts = false;

                if (String.IsNullOrEmpty(txtSearchField.Text))
                {
                    return;
                }

                string searchtext = txtSearchField.Text;

                findings = data?.cards?.FindAll(
                    x => x.ToString().Contains(txtSearchField.Text)
                );

                if( findings?.Any() ?? false)
                {
                    string msg = findings.Count switch {
                        1 => "FoundOne".LookupTranslation(),
                        _ => findings.Count + " " + "FoundEntries".LookupTranslation()
                    };
                    speaker.SpeakAsync(msg);
                    UpdateFields(findings.First());
                    hopPosition = 0;
                }
                else
                {
                    // Nichts mit abc gefunden.
                    speaker.SpeakAsync(String.Format("NothingFound".LookupTranslation(), searchtext));
                }
            }

            // next search position
            if (e.KeyCode == Keys.F3)
            {
                if(findings?.Count > hopPosition + 1)
                {
                    hopPosition++;
                    UpdateFields(findings[hopPosition]);
                }
                else
                {
                    speaker.SpeakAsync(
                        String.Format("ReachedItem".LookupTranslation(), hopPosition + 1, findings?.Count)
                    );
                }
            }
        }
    }
}