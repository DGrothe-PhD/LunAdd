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
    public partial class StandardForm : Form, ReadLarge
    {
        public static readonly SpeechSynthesizer speaker = new();
        public static readonly Dictionary<FieldType, String> LocalFieldNames = UIFieldNames.GermanFieldNames;
        public Prompt? SayThis;

        //TODO interface ReadLarge.MakePrompt
        //TODO make KeyDown virtual
        //TODO implement interface in Form1 as well
        public StandardForm()
        {
            KeyPreview = true;
        }
    }
}
