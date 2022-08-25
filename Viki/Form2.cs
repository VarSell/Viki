using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Viki
{
    public partial class Form2 : Form
    {
        internal static string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\data\config");
        internal static string[] oldConfig = File.ReadAllLines(file);
        public Form2()
        {
            InitializeComponent();
            ReadConfig();
        }
        internal void ReadConfig()
        {
            this.checkBox1.Checked = Convert.ToBoolean(oldConfig[0]);
            this.checkBox2.Checked = Convert.ToBoolean(oldConfig[1]);
            this.checkBox3.Checked = Convert.ToBoolean(oldConfig[2]);
            this.checkBox4.Checked = Convert.ToBoolean(oldConfig[3]);
            //this.checkBox5.Checked = Convert.ToBoolean(oldConfig[4]);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string[] config = new string[]
                {
                    Convert.ToString(this.checkBox1.Checked),
                    Convert.ToString(this.checkBox2.Checked),
                    Convert.ToString(this.checkBox3.Checked),
                    Convert.ToString(this.checkBox4.Checked),
                    Convert.ToString(this.checkBox5.Checked)
                };
            File.WriteAllLines(file, config);
            oldConfig = File.ReadAllLines(file);
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1.feedback = this.checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Form1.downloadSubtitles = this.checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Form1.useXMLTagsWhenAvailable = this.checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Form1.convertSubtitlesToSrt = this.checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //Form1.debug = this.checkBox5.Checked;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                this.checkBox1.Enabled = false;
                this.checkBox2.Enabled = true;
                this.checkBox3.Enabled = true;
                this.checkBox4.Enabled = true;
                this.checkBox5.Enabled = false;
            });
        }
    }
}