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
        public Form1()
        {
            InitializeComponent();
            this.pulseButton.MouseDown += new MouseEventHandler(this.pulseButton_Click);
            this.pulseButton.MouseUp += new MouseEventHandler(this.pulseButton_Release);
            this.endSequenceTimer.Tick += new EventHandler(this.check_Sequence);
            this.endSequenceTimer.Interval = 1000; // in miliseconds
            this.endSequenceTimer.Start();
        }

        private void pulseButton_Click(object sender, EventArgs e)
        {
            this.pulseStart = DateTime.Now;
            if (this.pulseStart - this.sequenceTime > TimeSpan.FromMilliseconds(300) && this.sequence != "")
            {
                Console.WriteLine(this.sequence);
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
                Console.WriteLine(this.sequence);
                this.sequence = "";
            }
        } 
    }
}
