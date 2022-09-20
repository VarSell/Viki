using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Viki
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        infoBox.Text = "This project currently has several dependancies.\nIf running into issues with Aria2c downloads and/or key retrieval, please make sure these requirements are all fullfilled.\n\nWindows\n - .NET 6.0 Runtime\n - x64 machine\n\nPython\n - requests\n - pyperclip\n - yt-dlp\n\nIf the application is still unable to retrieve the keys, please run '/src/tools/getwvkeys.py' manually via command line and see what causes the script to crash.";
                        break;
                    case 1:
                        infoBox.Text = "When enabled, a bug report containing the exception that occurred will be sent.\nNo personal information is contained.";
                        break;
                    case 2:
                        infoBox.Text = "When enabled, all available subtitles that surpass the Byte Limit will be downloaded and muxed into the Matroska.";
                        break;
                    case 3:
                        infoBox.Text = "When enabled, if TheMovieDataBase ID of the film is found, simple XML tags containing the TMDB ID will be generated and muxed into the Matroska.";
                        break;
                    case 4:
                        infoBox.Text = $"Counted in bytes.\nCurrent limit: {Properties.Settings.Default.SrtByteLimit.ToString()} ({(Properties.Settings.Default.SrtByteLimit / 1000).ToString()} KB)\nWhen muxing subtitles, if the .srt filesize is not greater than the limit set, it will be assumed as too incomplete and skipped from merge.";
                        break;
                    case 5:
                        infoBox.Text = "When enabled, additional progress / information will be shown.";
                        break;
                    case 6:
                        infoBox.Text = "When enabled, beta features will be enabled.\nCurrent feature is a new 'Start (Beta)' button that attempts batch downloads of all available episodes from a url in the given format: https://www.viki.com/tv/38634c-bad-girlfriend\nIf for some reason an episode is skipped during the mass download, redownload the singular episode using the normal 'Start' button. 'Start (Beta)' will attemp otherwise to redownload ALL the episodes.\nApplication restart is required for changes to take effect.";
                        break;
                    case 7:
                        infoBox.Text = "When enabled, Aria2c (used only for segmented manifests) output will be shown on screen. Unadvisable to enable this option, as it may sometimes break the control and shut down this application without any warning.\nUseful if running into errors when downloading the segmented streams for most Plus content.";
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
