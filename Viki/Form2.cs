using System.IO;
using static Viki.Config;

namespace Viki
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        internal void ReadConfig()
        {
            this.checkBox1.Checked = Properties.Settings.Default.Feedback;
            this.checkBox2.Checked = Properties.Settings.Default.DownloadSubtitles;
            this.checkBox3.Checked = Properties.Settings.Default.UseXMLTagsWhenAvailable;
            this.checkBox4.Checked = Properties.Settings.Default.Debug;
            this.checkBox5.Checked = Properties.Settings.Default.Beta;
            this.checkBox6.Checked = Properties.Settings.Default.Aria2cVerbose;
            this.textBox1.Text = Properties.Settings.Default.SrtByteLimit.ToString();
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SrtByteLimit = Int32.Parse(this.textBox1.Text);
            }
            catch
            {
                toolTip1.Show("Invalid Value.", this.textBox1);
                await Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    toolTip1.Hide(this.textBox1);
                });
                return;
            }

            Properties.Settings.Default.Beta = this.checkBox5.Checked;
            Beta = this.checkBox5.Checked;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Feedback = this.checkBox1.Checked;
            Properties.Settings.Default.Feedback = Feedback;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            DownloadSubtitles = this.checkBox2.Checked;
            Properties.Settings.Default.DownloadSubtitles = DownloadSubtitles;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            UseXMLTagsWhenAvailable = this.checkBox3.Checked;
            Properties.Settings.Default.UseXMLTagsWhenAvailable = UseXMLTagsWhenAvailable;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Debug = this.checkBox4.Checked;
            Properties.Settings.Default.Debug = Debug;
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (Beta != this.checkBox5.Checked)
            {
                this.pictureBox1.Visible = true;
            }
            else
            {
                this.pictureBox1.Visible = false;
            }
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            Aria2cVerbose = this.checkBox6.Checked;
            Properties.Settings.Default.Aria2cVerbose = Aria2cVerbose;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                this.checkBox1.Checked = false;
                this.checkBox2.Checked = true;
                this.checkBox3.Checked = true;
                this.checkBox4.Checked = false;
                this.checkBox5.Checked = false;
                this.checkBox6.Checked = false;
                this.textBox1.Text = "7000";
            });
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(this.label4.Text) { UseShellExecute = true });
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ReadConfig();
        }

        private async void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            if (this.pictureBox1.Visible)
            {
                toolTip1.Show("Application needs to be restarted.", this.pictureBox1);
                await Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    this.pictureBox1.Visible = false;
                });
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Help", this.pictureBox2);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Text == "Information")
                {
                    form.Focus();
                    return;
                }
            }

            Form3 f = GetF();
            f.Show();
        }

        private static Form3 GetF()
        {
            return new Form3();
        }
    }
}