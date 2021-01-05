using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Text.RegularExpressions;

namespace Morse
{
    public partial class Form1 : Form
    {
        private DateTime sequenceTime = DateTime.Now;
        private Timer endSequenceTimer = new Timer();
        private string sequence = "";
        private DateTime pulseStart = DateTime.Now;
        private DateTime pulseEnd = DateTime.Now;
        private readonly SoundPlayer beeper = new SoundPlayer(Path.Combine(Environment.CurrentDirectory, @"", "880_Sine_wave.wav"));
        private List<CharacterInfo> definitions = new List<CharacterInfo>();
        public Form1()
        {
            InitializeComponent();
            this.pulseButton.MouseDown += new MouseEventHandler(this.pulseButton_Click);
            this.pulseButton.MouseUp += new MouseEventHandler(this.pulseButton_Release);
            this.endSequenceTimer.Tick += new EventHandler(this.check_Sequence);
            this.richTextBox1.TextChanged += new EventHandler(this.check_Text);
            this.endSequenceTimer.Interval = 1000; // in miliseconds
            this.endSequenceTimer.Start();

            this.definitions.Add(new CharacterInfo("A", ".-"));
            this.definitions.Add(new CharacterInfo("B", "-..."));
            this.definitions.Add(new CharacterInfo("C", "-.-."));
            this.definitions.Add(new CharacterInfo("D", "-.."));
            this.definitions.Add(new CharacterInfo("E", "."));
            this.definitions.Add(new CharacterInfo("F", "..-."));
            this.definitions.Add(new CharacterInfo("G", "--."));
            this.definitions.Add(new CharacterInfo("H", "...."));
            this.definitions.Add(new CharacterInfo("I", ".."));
            this.definitions.Add(new CharacterInfo("J", ".---"));
            this.definitions.Add(new CharacterInfo("K", "-.-"));
            this.definitions.Add(new CharacterInfo("L", ".-.."));
            this.definitions.Add(new CharacterInfo("M", "--"));
            this.definitions.Add(new CharacterInfo("N", "-."));
            this.definitions.Add(new CharacterInfo("O", "---"));
            this.definitions.Add(new CharacterInfo("P", ".--."));
            this.definitions.Add(new CharacterInfo("Q", "--.-"));
            this.definitions.Add(new CharacterInfo("R", ".-."));
            this.definitions.Add(new CharacterInfo("S", "..."));
            this.definitions.Add(new CharacterInfo("T", "-"));
            this.definitions.Add(new CharacterInfo("U", "..-"));
            this.definitions.Add(new CharacterInfo("V", "...-"));
            this.definitions.Add(new CharacterInfo("W", ".--"));
            this.definitions.Add(new CharacterInfo("X", "-..-"));
            this.definitions.Add(new CharacterInfo("Y", "-.--"));
            this.definitions.Add(new CharacterInfo("Z", "--.."));
            this.definitions.Add(new CharacterInfo("1", ".----"));
            this.definitions.Add(new CharacterInfo("2", "..---"));
            this.definitions.Add(new CharacterInfo("3", "...--"));
            this.definitions.Add(new CharacterInfo("4", "....-"));
            this.definitions.Add(new CharacterInfo("5", "....."));
            this.definitions.Add(new CharacterInfo("6", "-...."));
            this.definitions.Add(new CharacterInfo("7", "--..."));
            this.definitions.Add(new CharacterInfo("8", "---.."));
            this.definitions.Add(new CharacterInfo("9", "----."));
            this.definitions.Add(new CharacterInfo("0", "-----"));
        }

        private void pulseButton_Click(object sender, EventArgs e)
        {
            this.pulseStart = DateTime.Now;
            if (this.pulseStart - this.sequenceTime > TimeSpan.FromMilliseconds(300) && this.sequence != "")
            {
                this.getCharacter();
                this.sequence = "";
            }
            beeper.Play();
        }

        private void pulseButton_Release(object sender, EventArgs e)
        {
            beeper.Stop();
            this.pulseEnd = DateTime.Now;
            this.sequenceTime = DateTime.Now;
            TimeSpan beepTime = this.pulseEnd - this.pulseStart;
            //Console.WriteLine(beepTime);
            if (beepTime > TimeSpan.FromMilliseconds(150))
            {
                this.sequence += "-";
            }
            else
            {
                this.sequence += ".";
            }
        }

        private void check_Sequence(object sender, EventArgs e)
        {
            if (DateTime.Now - this.sequenceTime > TimeSpan.FromMilliseconds(1000) && this.sequence != "")
            {
                this.getCharacter();
                this.sequence = "";
            }
        }

        private void getCharacter()
        {
            Console.WriteLine(this.sequence);
            CharacterInfo character = this.definitions.SingleOrDefault(a => a.morse == this.sequence);
            if (character != null)
            {
                this.richTextBox1.Text += character.character;
            }
            else if (this.sequence.Length > 7 && this.richTextBox1.Text.Length > 0)
            {
                this.richTextBox1.Text = this.richTextBox1.Text.Remove(this.richTextBox1.Text.Length - 1);
            }
        }

        private void check_Text(object sender, EventArgs e)
        {
            Regex rgx = new Regex("[^A-Z0-9 -]");
            this.richTextBox1.Text = rgx.Replace(this.richTextBox1.Text, "");
        }
    }

    public class CharacterInfo
    {
        public string character;
        public string morse;
        public List<int> timeSignatures = new List<int>();

        public CharacterInfo (string character, string morse)
        {
            this.character = character;
            this.morse = morse;
            foreach (char c in this.morse)
            {
                if(c == '.')
                {
                    this.timeSignatures.Add(150);
                }
                else
                {
                    this.timeSignatures.Add(300);
                }
            }
        }
    }
}
