using System.IO;
using static Viki.Config;

namespace Viki
{
    public partial class Form2 : Form
    {
        internal static string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\data\config");
        internal static byte[] config = TryFileRead();
        public Form2()
        {
            InitializeComponent();
        }

        internal static byte[] TryFileRead()
        {
            try
            {
                return File.ReadAllBytes(file);
            }
            catch
            {
                File.WriteAllBytes(file, new byte[] { 0x0, 0x1, 0x1, 0x0 });
            }
            return File.ReadAllBytes(file);
        }
        internal void ReadConfig()
        {
            this.checkBox1.Checked = Convert.ToBoolean(config[0]);
            this.checkBox2.Checked = Convert.ToBoolean(config[1]);
            this.checkBox3.Checked = Convert.ToBoolean(config[2]);
            this.checkBox4.Checked = Convert.ToBoolean(config[3]);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            byte[] config = new byte[]
                {
                    Convert.ToByte(this.checkBox1.Checked),
                    Convert.ToByte(this.checkBox2.Checked),
                    Convert.ToByte(this.checkBox3.Checked),
                    Convert.ToByte(this.checkBox4.Checked)
                };
            File.WriteAllBytes(file, config);
            Form2.config = File.ReadAllBytes(file);
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Feedback = this.checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            DownloadSubtitles = this.checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            UseXMLTagsWhenAvailable = this.checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Debug = this.checkBox4.Checked;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                this.checkBox1.Checked = false;
                this.checkBox2.Checked = true;
                this.checkBox3.Checked = true;
                this.checkBox4.Checked = false;
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
    }
}