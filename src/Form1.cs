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
        private Dictionary<int, VCard>? findingsWithIndices;
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
        private void UpdateFields(VCard? currentCard, int? index = null)
        {
            ignoreVCEvent = true;
            try
            {
                VCard = currentCard;
            }
            catch (Exception)
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

            if (index == null)
            {
                txtSearchField.Text = String.Format(
                    "DisplayCardNumber".LookupTranslation(),
                    currentIndex, data?.cards.Count.ToString() ?? ""
                    );
                numIndex.Value = currentIndex;
            }
            else
            {
                currentIndex = (int)index + 1;
            }

            btnForward.Visible = (currentIndex < data?.cards.Count);
            btnBack.Visible = (currentIndex > 1);
            ignoreVCEvent = false;
        }

        private void BtnForward_Click(object sender, EventArgs e)
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

        private void BtnBack_Click(object sender, EventArgs e)
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

        private void NumIndex_ValueChanged(object sender, EventArgs e)
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

        private void BtnOpening_Click(object sender, EventArgs e)
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
                findingsWithIndices?.Clear();
                disabledShortcuts = true;
                txtSearchField.Select();
                txtSearchField.Clear();
                return;
            }

            // while typing search text don't flip card entries but allow cursor to move
            if (e.KeyCode == Keys.Right && !disabledShortcuts)
            {
                FlipForward();
            }
            if (e.KeyCode == Keys.Left && !disabledShortcuts)
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

                // Select all cards that match, with their index.
                // User can scroll through the next `pages` from an entry found
                findingsWithIndices = data!.cards!.Select((c, i) => new { card = c, Index = i })
                    .Where(x => x.ToString()!.Contains(txtSearchField.Text, StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(x => x.Index, x => x.card);

                if (findingsWithIndices?.Any() ?? false)
                {
                    currentIndex = findingsWithIndices!.First().Key;

                    string msg = findingsWithIndices!.Count switch
                    {
                        1 => "FoundOne".LookupTranslation(),
                        _ => findingsWithIndices.Count + " " + "FoundEntries".LookupTranslation()
                    };
                    speaker.SpeakAsync(msg);

                    UpdateFields(findingsWithIndices?.First().Value, findingsWithIndices?.First().Key);
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
                if (!findingsWithIndices?.Any() ?? true)
                {
                    speaker.SpeakAsync(
                        String.Format("EmptyItemsList".LookupTranslation())
                    );
                }
                else if (findingsWithIndices?.Count > hopPosition + 1)
                {
                    hopPosition++;
                    var f = findingsWithIndices.ElementAtOrDefault(hopPosition);
                    UpdateFields(f.Value, f.Key);
                }
                else
                {
                    speaker.SpeakAsync(
                        String.Format("ReachedItem".LookupTranslation(), hopPosition + 1, findingsWithIndices?.Count)
                    );
                    hopPosition = 0;
                    var f = findingsWithIndices!.First();
                    UpdateFields(f.Value, f.Key);
                }
            }
        }
    }
}