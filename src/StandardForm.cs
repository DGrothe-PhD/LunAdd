using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LunAdd
{
    public partial class StandardForm : Form
    {
        public static readonly SpeechSynthesizer speaker = new();
        public static readonly Dictionary<FieldType, String> LocalFieldNames = UIFieldNames.GermanFieldNames;

        public StandardForm()
        {
            KeyPreview = true;
        }
    }
}
