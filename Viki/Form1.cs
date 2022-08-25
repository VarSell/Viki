using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using System;
using System.Threading;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V85.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V85.Network;
using Newtonsoft.Json;
using System.Diagnostics;
using HtmlAgilityPack;
using RestSharp;
using OpenQA.Selenium.Interactions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Net.Mail;
using static Viki.Other;


namespace Viki
{
    public partial class Form1 : Form
    {
        //public static Stopwatch sw = Stopwatch.StartNew();
        public static string key = String.Empty;
        public static string procLog = String.Empty;
        public static string filePath = String.Empty;
        public static IWebDriver driver;// = Form1.GetDriver();
        public static bool isLogged = false;
        public static bool gwvk = false; // set to true when sharing
        public static bool showProc_2 = false;
        public static bool driverStarted = false;

        public static Viki v = new Viki();
        public static bool isLoaded = false;
        public Form1()
        {
            InitializeComponent();

            this.vikiLink.Text = "https://www.viki.com/videos/1115053v";
            //Directory.SetCurrentDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src"));
            //Log("Starting driver.");
            //Log(sw.ElapsedMilliseconds.ToString());
        }
        /*public void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            _=SendBugReport(e.Exception, new StackTrace(true));
        }

        public void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _=SendBugReport(e.ExceptionObject as Exception, new StackTrace(true));
        }*/

        public void Download(string url, string outFile)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
#pragma warning disable CS4014
                Monitor(outFile, Convert.ToDouble(DetermineByteSize(url))); // this call is not meant to be awaited
#pragma warning restore CS4014
                client.DownloadFile(url, outFile);
            }
        }
        internal static string Culture(string new_cult)
        {
            try
            {
                string current_cult = System.Globalization.CultureInfo.CurrentCulture.Name;
                System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo(new_cult);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                new_cult = System.Globalization.CultureInfo.CurrentCulture.DisplayName;
                System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo(current_cult);
                if (new_cult.Length == 2)
                    return "und";
                return new_cult;
            }
            catch (Exception)
            {
                if (new_cult == ("zh_TW") || new_cult == ("zh_CN"))
                {
                    return Culture("zh");
                }
                return "und";
            }


        }
        public static ChromeDriver GetDriver(bool anon = false)
        {
            ChromeOptions options = new ChromeOptions();

            //Following Logging preference helps in enabling the performance logs
            options.SetLoggingPreference("performance", LogLevel.All);

            //Based on your need you can change the following options
            options.AddUserProfilePreference("intl.accept_languages", "en-US");
            options.AddUserProfilePreference("disable-popup-blocking", "true");
            options.AddArgument("test-type");
            options.AddArgument("--allow-running-insecure-content");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=0,0");//1920,1080
            //options.AddArgument("--profile-directory=\"Profile 1\"");
            //options.AddArgument(@"user-data-dir=C:\Users\User\AppData\Local\Google\Chrome\User Data\Profile 1");


            
            options.AddArgument("no-sandbox");
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            options.LeaveBrowserRunning = true;

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            driverStarted = true;
            return new ChromeDriver(chromeDriverService, options);
            /*
            if (!anon)
            {
                return new ChromeDriver(chromeDriverService, options);
            }
            else
            {
                return (UndetectedChromeDriver) new ChromeDriver(chromeDriverService, options);
                //return UndetectedChromeDriver.Create(browserExecutablePath: @"C:\Program Files\Google\Chrome\Application\chrome.exe", driverExecutablePath: @"C:\Users\User\source\repos\TSelenium\TSelenium\bin\Debug\net6.0\chromedriver.exe", options: options);
            }*/
        }
        public void Login(IWebDriver driver)
        {
            if (isLogged)
            {
                Log("Logged in.");
                LoadPage(driver, this.vikiLink.Text.Trim());
                return;
            }
            string[] credentials = File.ReadAllLines("credentials.txt");
            string email = string.Empty;
            string passwd = string.Empty;

            try
            {
                email = credentials[0];
                passwd = credentials[1];

                Log("Logging in.");
                LoadPage(driver, $"https://www.viki.com/sign-in?return_to={this.vikiLink.Text.Trim()}?auto_play=1");
                string[] t = { email, passwd };
                int i = 0;
                foreach (var c in driver.FindElements(By.CssSelector(".sc-dtDOqo.gELvNZ")))
                {
                    c.SendKeys(t[i]);
                    i++;
                }
                driver.FindElements(By.CssSelector(".sc-dtDOqo.gELvNZ"))[1].SendKeys(OpenQA.Selenium.Keys.Return);
                isLogged = true;
            }
            catch (IndexOutOfRangeException)
            {
                Log("Skipping Login.");
                LoadPage(driver, this.vikiLink.Text);
            }
        }
        public void LoadPage(IWebDriver driver, string uri)
        {
            //IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
            driver.Navigate().GoToUrl(uri);
            //je.ExecuteScript("document.head.parentNode.removeChild(document.head);");
            //driver.Manage().Window.Minimize();
        }
        [STAThread] // doesn't do shit
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                Record.SendBugReport(ex, new StackTrace(true), LogBox);
            }
            /*if (!NetworkIsAvailable())
            {
                Log("No connection.");
                return;
            }
            string prevText = Clipboard.GetText();
            bool _paste = false;
            await Task.Run(() =>
            {
                LoadViki();
                isLoaded = true;
                _paste = true;
                
            });
            while (!_paste)
            {}
            this.keyPair.Text = Clipboard.GetText().Trim();
            if (String.IsNullOrEmpty(this.keyPair.Text))
            {
                Log("Failed Data Retrieval.");
            }
            else
            {
                KeyValidation(this.keyPair.Text);
            }
            this.button1.Enabled = true;*/
        }
        public void LoadViki()
        {
            if (isLoaded)
            {
                Log("Already Parsed.");
                return;
            }
            //this.button1.Enabled = false;

            string uri = this.vikiLink.Text;

            //Creating Chrome driver instance

            //Log("Starting driver.");
            //driver = GetDriver();
            //driver.Manage().Window.Minimize();

            //UndetectedChromeDriver driver = (UndetectedChromeDriver) GetDriver(true);
            Login(driver);


            //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            //string title = (string)js.ExecuteScript("document.getElementsByClassName(\"vjs-big-play-button\")[0].click();");
            
            Log("Parsing metadata.");

            v.VParse(uri);

        retry:

            try
            {
                //driver.FindElement(By.CssSelector(".vkp-next-button.vkp-svg")).Click();
                driver.FindElement(By.Id("html5_player_id")).Click();
            }
            catch
            {
                //Thread.Sleep(1700);
                goto retry;
            }

            Log("Scanning network.");
            string[] links = GetLinksFromDriver(driver);

            Log("Terminating tab.");
            //driver.Quit();
            //LoadPage(driver, "about:blank");
            ResetPage(driver);

            Log("Downloading manifest.");
            var client = new RestClient(links[0]);
            var request = new RestRequest();
            var response = client.Execute(request);

            File.WriteAllText("dump\\_dump", response.Content);
            string[] _temp = File.ReadAllLines("dump\\_dump");

            string[] streams = new string[2];

            // if the high manifest is present
            foreach (string ln in _temp)
            {
                if (ln.Contains("high") && ln.Contains(".mpd"))
                {
                    Log("Found high manifest.");
                    string l = ln.Split("<BaseURL>")[1].Split("</BaseURL>")[0];
                    using (var client1 = new System.Net.WebClient())
                    {
                        Stream stream = client1.OpenRead(l);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        streams = ParseStreamsLinks(content.Split("\n"));
                    }
                }
            }

            Log($"Audio Filesize: {(((DetermineByteSize(streams[0]) / 1024)) / 1024).ToString()} MiB");
            Log($"Video Filesize: {(((DetermineByteSize(streams[1]) / 1024)) / 1024).ToString()} MiB");

            this.audioUrl.Text = streams[0];
            this.videoUrl.Text = streams[1];



            //Here I'm assuming for now that there will always be present a high manifest. It doesn't take into
            // account that if there is no high present, no links will be parsed.

            Log("Requesting keys.");
            _=ScanSubs();
            if (!gwvk)
            {
                string[] oL3 = File.ReadAllLines(@"wsk\l3.py");
                oL3[0] = @$"lic_url = '{links[1]}'";
                oL3[1] = $@"MDP_URL = '{links[0]}'";
                oL3[22] = "pssh = get_pssh(MDP_URL)";
                File.WriteAllLines(@"wsk\l3.py", oL3);
                SoftWare(@"wsk/l3.py");
            }
            else
            {
                string[] file = File.ReadAllLines(@"scripts\getwvkeys.py");
                file[10] = $"licUrl = '{links[1]}'";
                file[11] = $"manifest = '{links[0]}'";
                file[12] = "pssh = get_pssh(manifest)";
                file[21] = "    return {\"Connection\": \"keep-alive\", \"accept\": \"*/*\"}";
                File.WriteAllLines(@"scripts\getwvkeys.py", file);
                SoftWare(@"scripts\getwvkeys.py");
            }
            string pYear = DateTime.Parse(v.ReleaseYear).Year.ToString();
            this.releaseName.Text = String.Concat(v.Title.Trim());//.Replace(" ", "."), $".{pYear}.", "1080p.VIKI.WEB-DL.AAC.2.0.H.264-ODiUM");
                                                                  //Log(v.Release); //Not needed?
            this.headerMeta.Text = $"{v.TitleMeta} ({pYear})";

            TMDBAPIRequest(true);

            //v.TitleMeta = this.headerMeta.Text;
            this.languageISO.Text = v.Language;
            this.releaseYear.Text = v.ReleaseYear;
            isLoaded = true;
            //string decryptionKey = Clipboard.GetText(); // doesn't do shit

            //this.keyPair.Focus();
            //SendKeys.SendWait("^{v}");
            //this.keyPair.Text = decryptionKey; // doesn't do shit
        }
        public long DetermineByteSize(string url)
        {
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                client.OpenRead(url);
                Int64 bytes_total = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                return bytes_total;
            }
            catch
            {
                return 0;
            }
        }
        public void TMDBAPIRequest(bool viki)
        {
            try
            {
                string[] details = new string[5];
                string id = string.Empty;
                string kind = "movie";
                if (viki)
                {
                    string formattedYear = DateTime.Parse(v.ReleaseYear).Year.ToString();
                    try
                    {
                        id = TMDB_shifter(String.Concat(v.TitleMeta.Split("-")[0].Trim(), " y:", formattedYear), kind);
                    }
                    catch
                    {
                        kind = "tv";
                        id = TMDB_shifter(String.Concat(v.TitleMeta.Split("-")[0].Trim(), " y:", formattedYear), kind);
                    }
                }
                else
                {
                    /*try
                    {
                        id = TMDB_shifter(String.Concat(m.Title, " y:", m.Year), kind);
                    }
                    catch
                    {
                        kind = "tv";
                        id = TMDB_shifter(String.Concat(m.Title, " y:", m.Year), kind);
                    }*/
                }

                details = TMDBRequest(kind, id);

                this.tmdb.Text = id;
            }
            catch
            {
                Log("TMDB API Error.");
                this.backDrop.Image = Properties.Resources.murderCat;
                this.backDrop.Refresh();
            }
        }
        public string[] ParseStreamsLinks(string[] manifest)
        {
            // this method also assumes that the high manifest is passes to it, thus the link concats
            string[] links = new string[2];
            foreach (string ln in manifest)
            {
                if (ln.Contains("_dash_high_1080p") && ln.Contains("audio"))
                {
                    string l = ln.Split("<BaseURL>")[1].Split("</BaseURL>")[0];
                    l = String.Concat("https://cloudfront.viki.net/", l.Split("_")[0], "/dash/", l);
                    links[0] = l;
                }
                if (ln.Contains("mov_drm") && ln.Contains("audio"))
                {
                    string l = ln.Split("<BaseURL>")[1].Split("</BaseURL>")[0];
                    l = String.Concat("https://cloudfront.viki.net/", l.Split("_")[0], "/dash/", l);
                    links[0] = l;
                }
                // _dash_high_1080p_mov_drm
                /*if (line.Contains("_dash_high_720p") && line.Contains("video"))
                {
                    videoUrl = CleanLink(line);
                }*/
                if (ln.Contains("_dash_high_1080p") && ln.Contains("video"))
                {
                    string l = ln.Split("<BaseURL>")[1].Split("</BaseURL>")[0];
                    l = String.Concat("https://cloudfront.viki.net/", l.Split("_")[0], "/dash/", l);
                    links[1] = l;
                }
                if (ln.Contains("mov_drm") && ln.Contains("video"))
                {
                    string l = ln.Split("<BaseURL>")[1].Split("</BaseURL>")[0];
                    l = String.Concat("https://cloudfront.viki.net/", l.Split("_")[0], "/dash/", l);
                    links[1] = l;
                }
            }
            return links;
        }
        public void Log(string alert, int id = 0)
        {
            DateTime time = DateTime.Now;
            string format = "HH:mm:ss";
            alert = String.Concat("[", time.ToString(format), "] ", alert+Environment.NewLine);
            if (id == 0)
            {
                this.LogBox.AppendText(alert);
                this.LogBox.ScrollToCaret();
            }
            else if (id == 1)
            {
                if (alert.Contains("[S]"))
                {
                    alert = alert.Replace("[S]", String.Empty);
                    this.LogBox1.SelectionFont = new Font("Arial", 9, FontStyle.Strikeout);
                }
                this.LogBox1.AppendText(alert);
                this.LogBox1.ScrollToCaret();
            }
        }
        public static void SoftWare(string args, string executable = "python.exe")
        {
            procLog = string.Empty;
            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo(executable, args)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };


            proc.ErrorDataReceived += proc_DataReceived;
            proc.OutputDataReceived += proc_DataReceived;
            proc.Start();

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();



            proc.WaitForExit();
        }

        public static void proc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                procLog += e.Data;
            }
        }
        public void SoftWare_2(string args)
        {
            var proc_2 = new Process()
            {
                StartInfo = new ProcessStartInfo("cmd.exe", "/c " + args)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };


            proc_2.ErrorDataReceived += proc_2_DataReceived;
            proc_2.OutputDataReceived += proc_2_DataReceived;
            proc_2.Start();

            proc_2.BeginErrorReadLine();
            proc_2.BeginOutputReadLine();



            proc_2.WaitForExit();
        }

        public void proc_2_DataReceived(object sender, DataReceivedEventArgs e)
        {
            //LogBox.ScrollToCaret();
            LogBox.ScrollToCaret();
            //35cfc571679545829b322475ac8af898:cf7c7fcaaf2380e47d6f375a5adc05ae
            //35cfc571679545829b322475ac8af898:cf7c7fcaaf2380e47d6f375a5adc05ae
            if (e.Data != null)
            {
                if (LogBox.Text.Length > 7000)
                {
                    LogBox.Clear();
                }
                if (showProc_2)
                {
                    Log(e.Data);
                }
            }

        }
        public string[] GetLinksFromDriver(IWebDriver driver)
        {
            string mpd = String.Empty;
            string lic = String.Empty;
            bool foundMPD = false;
            bool foundLIC = false;

            /////////

            while (!foundMPD || !foundLIC)
            {
                foreach (var log in driver.Manage().Logs.GetLog("performance"))
                {
                    if (log.ToString().Contains("manifest") && foundMPD == false)
                    {
                        foundMPD = true;
                        string[] msgArr = log.Message.Split(",");
                        foreach (string ln in msgArr)
                        {
                            if (ln.Contains("manifest"))
                            {
                                Log("Found manifest.");
                                string manifest = ln.Replace("\"", String.Empty).Split("url:")[1].Split("}")[0];
                                mpd = manifest;
                            }
                        }
                    }
                    if (log.ToString().Contains("license") && foundLIC == false)
                    {
                        foundLIC = true;
                        string[] msgArr = log.Message.Split(",");
                        foreach (string ln in msgArr)
                        {
                            if (ln.Contains("license"))
                            {
                                Log("Found license.");
                                string license = ln.Replace("\"", String.Empty).Split("url:")[1].Split("}")[0];
                                lic = license;
                            }
                        }
                    }
                }
            }

            /////////

            /*while (!foundMPD || !foundLIC)
            {
                var log = driver.Manage().Logs.GetLog("performance");
                for (int i = 0; i < log.Count; i++)
                {
                    if (log[i].ToString().Contains("manifest") && foundMPD == false)
                    {
                        foundMPD = true;
                        string[] msgArr = log[i].Message.Split(",");
                        foreach (string ln in msgArr)
                        {
                            if (ln.Contains("manifest"))
                            {
                                Log("Found manifest.");
                                string manifest = ln.Replace("\"", String.Empty).Split("url:")[1].Split("}")[0];
                                mpd = manifest;
                            }
                        }
                    }
                    if (log[i].ToString().Contains("license") && foundLIC == false)
                    {
                        foundLIC = true;
                        string[] msgArr = log[i].Message.Split(",");
                        foreach (string ln in msgArr)
                        {
                            if (ln.Contains("license"))
                            {
                                Log("Found license.");
                                string license = ln.Replace("\"", String.Empty).Split("url:")[1].Split("}")[0];
                                lic = license;
                            }
                        }
                    }
                }
            }*/

            return new string[] { mpd, lic };
        }

        public async Task Monitor(string file, double size)
        {
            await Task.Run(() =>
            {
                while (!File.Exists(file))
                {
                    Thread.Sleep(300);
                }
                while (true)
                {
                    double percentage = new FileInfo(file).Length * 100 / size;
                    percentage = Math.Round(percentage, 0);
                    this.progressBar1.Value = (int)percentage;
                    if (percentage == 100)
                        return;
                    Thread.Sleep(700);
                }
            });
        }
        public void ResetPage(OpenQA.Selenium.IWebDriver drv)
        {
            drv.Navigate().GoToUrl("about:blank");
        }
        internal string GetPSSHFromFragment(string manifest)
        {
            Log("Downloading fragment.");
            SoftWare_2($"yt-dlp --test --allow-u -f bv -o \"dump\\fragment.mp4\" \"{manifest}\"");
            Log("Retrieving initData.");
            SoftWare(@"scripts\PSSHFromFrag.py");
            File.Delete(@"dump\fragment.mp4");
            return Clipboard.GetText();
        }
        internal string GetInitDataFromKID(string KID)
        {
            List<byte> data = new List<byte>() { 0x00, 0x00, 0x00, 50, 112, 115, 115, 104, 0x00, 0x00, 0x00, 0x00, 237, 239, 139, 169, 121, 214, 74, 206, 163, 200, 39, 220, 213, 29, 33, 237, 0x00, 0x00, 0x00, 0x12, 0x12, 0x10 };
            data.AddRange(Convert.FromHexString(KID.Replace("-", "")));
            return Convert.ToBase64String(data.ToArray());
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Directory.SetCurrentDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src"));
            //_=ScanSubs();
            //return;
            if (!driverStarted)
            {
                
                Log("Starting driver.");
                driver = Form1.GetDriver();
            }
            //return;
            Log(new Uri(this.vikiLink.Text).Host);
            try
            {
                if (!NetworkIsAvailable())
                {
                    Log("No connection.");
                    return;
                }
                if (new Uri(this.vikiLink.Text).Host == "www.iq.com")
                {
                    Log("IQIYI beta.");
                    await Task.Run(() =>
                    {
                        LoadIQIYI();
                    });
                    return;
                }
                
                await Task.Run(() =>
                {
                    foreach (string file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src\\dump")))
                    {
                        //File.Delete(file);
                    }

                    LoadViki();
                    _=EnableButtons(false);
                });

                this.keyPair.Text = Clipboard.GetText().Trim();

                isLoaded = false;

                KeyValidation(this.keyPair.Text);

                await Task.Run(() =>
                {
                    Log("Downloading Audio");
                    Download(this.audioUrl.Text, "dump\\encAudio.mp4");
                    Log("Downloading Video");
                    Download(this.videoUrl.Text, "dump\\encVideo.mp4");

                    string folder;
                    if (String.IsNullOrEmpty(v.Release))
                    {
                        folder = this.releaseName.Text;
                    }
                    else
                    {
                        folder = v.Release;
                    }
                    try
                    {
                        folder = folder.Split("-")[0].Trim();
                    }
                    catch
                    { }

                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", folder);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    v.Release = this.releaseName.Text;
                    v.TitleMeta = this.headerMeta.Text;

                    LoadXmlTags();

                    Log("Decrypting Audio.");
                    v.Decrypt("dump\\encAudio.mp4", "dump\\decAudio.mp4", this.keyPair.Text);
                    Log(procLog);
                    Log("Decrypting Video");
                    v.Decrypt("dump\\encVideo.mp4", "dump\\decVideo.mp4", this.keyPair.Text);
                    Log(procLog);
                    Log("Merging.");
                    v.Merge(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src\\dump\\decAudio.mp4"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src\\dump\\decVideo.mp4"), filePath);
                    Log(procLog);

                    Log("Tagging metadata.");
                    v.Tag(Path.Combine(filePath, this.releaseName.Text), 1); // video stream
                    v.Tag(Path.Combine(filePath, this.releaseName.Text), 2); // audio stream

                    Log(procLog);
                    Log("Cleaning temp.");
                    //Other.DeleteTemporaryData();
                    _=EnableButtons(true);
                });
            }
            catch (Exception ex)
            {
                Record.SendBugReport(ex, new StackTrace(true), LogBox);
            }
        }
        public void LoadIQIYI()
        {
            SoftWare_2($"yt-dlp --cookies \"iqiyiCookies.txt\" --all-subs --sub-format \"srt\" --remux-video \"mkv\" --embed-subs --clean {this.vikiLink.Text}");
        }
        // START
        internal static bool feedback = false;
        internal static bool downloadSubtitles = true;
        internal static bool showPoster = true;
        internal static bool useXMLTagsWhenAvailable = true;
        internal static bool convertSubtitlesToSrt = true;
        //internal static bool debug = false; // show lots of info lol
        // END
        private async void button3_Click(object sender, EventArgs e)
        {
            var f = new Form2();
            f.Show();
        }
        public void KeyValidation(string fullKey)
        {
            if (String.IsNullOrEmpty(fullKey))
            {
                Log("Failed Data Retrieval.");
            }
            else
            {
                try
                {
                    string kid = fullKey.Split(":")[0];
                    string key = fullKey.Split(":")[1];

                    if ((kid.Length != 32) && key.Length != 32)
                    {
                        Log("Incorrect key Length");
                    }
                }
                catch
                {
                    Log("Incorrect key format.");
                }
            }

        }
        public async Task ScanSubs()
        {
            await Task.Run(() =>
            {
                this.button3.Enabled = false;
                //string[] extensions = { ".mp4", ".srt" };
                //var files = Directory.GetFiles(srcDIR, ".").Where(f => Array.Exists(extensions, e => f.EndsWith(e))).ToArray();
                //foreach (string file in Directory.GetFiles($@"{AppDomain.CurrentDomain.BaseDirectory}src\dump\"))
                //{
                //    File.Delete(file);
                //}
                Log("Scanning.", 1);
                string args = $"/c yt-dlp --all-subs --skip-download --allow-u --sub-format \"srt\" \"{this.vikiLink.Text}\" -P \"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump")}\"";
                SoftWare(args, "cmd.exe");
                string[] subs = Directory.GetFiles($@"{AppDomain.CurrentDomain.BaseDirectory}src\dump\", "*.srt");

                foreach (string file in subs)
                {
                    string cult = ((Path.GetFileNameWithoutExtension(file)).Split("]")[1].Replace(".", string.Empty));
                    if (new FileInfo(file).Length > 7000)
                    {
                        string lang = Culture(cult);
                        Log(lang, 1);
                    }
                    else
                    {
                        Log($"[S] {Culture(cult)}", 1);
                    }
                }
                this.button3.Enabled = true;
            });
        }
        public async Task EnableButtons(bool enabled)
        {
            await Task.Run(() =>
            {
                this.button1.Enabled = enabled;
                this.button2.Enabled = enabled;
                this.button3.Enabled = enabled;
            });
        }
        public static List<String> tags = new List<String>();
        public void LoadXmlTags()
        {
            if (String.IsNullOrEmpty(this.tmdb.Text))
            {
                string _file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump\metadata.xml");
                if (File.Exists(_file))
                {
                    File.Delete(_file);
                }
                return;
            }
            string[] _tags = { "\"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\"",
            "<!DOCTYPE Tags SYSTEM \"matroskatags.dtd\">",
            "<Tags>",
            "    <Tag> <!-- Movie (simple tagging) -->",
            "        <Targets>",
            "            <TargetTypeValue>70</TargetTypeValue>",
            "        </Targets>",
            "        <Simple>",
            "            <Name>TMDB</Name>",
            $"            <String>movie/{this.tmdb.Text.Trim()}</String>",
            "        </Simple>",
            "    </Tag>",
            "</Tags>" };

            for (int i = 0; i < _tags.Length; i++)
            {
                tags.Add(_tags[i]);
            }
            File.WriteAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump\metadata.xml"), tags);
            if (this.releaseName.Text.Contains("Episode"))
            {

            }
        }

        public static bool NetworkIsAvailable()
        {
            // only recognizes changes related to Internet adapters
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                // however, this will include all adapters -- filter by opstatus and activity
                System.Net.NetworkInformation.NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                return (from face in interfaces
                        where face.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up
                        where (face.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Tunnel) && (face.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Loopback)
                        select face.GetIPv4Statistics()).Any(statistics => (statistics.BytesReceived > 0) && (statistics.BytesSent > 0));
            }

            return false;
        }

        private void vikiLink_TextChanged(object sender, EventArgs e)
        {
            isLoaded = false;
        }
        public string TMDB_shifter(string search, string kind)
        {

            string url = string.Concat("https://www.themoviedb.org/search?query=", search);
            string raw_data = simple_request(url);
            //split patter = " href="/tv/ -List
            string[] match = raw_data.Split(new string[] { $"class=\"result\" href=\"/{kind}/" }, StringSplitOptions.RemoveEmptyEntries);
            match[1] = match[1].Substring(0, match[1].IndexOf("\"") + 1);
            match[1] = (match[1].Remove(match[1].Length - 1)).Trim();
            Log($"TMDB ID: {match[1]}");
            return match[1];

        }
        public string simple_request(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest();
            var response = client.Execute(request);

            return response.Content;
        }
        public string[] TMDBRequest(string kind, string id)
        {
            string url = $"https://api.themoviedb.org/3/{kind}/{id}?api_key=dc0141096c9d0bb09bdde4025387888b";
            var client = new RestClient(url);
            var request = new RestRequest();
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string[] details = new string[7];
                API.Rootobject result = JsonConvert.DeserializeObject<API.Rootobject>(response.Content);
                API.Production_Countries P_C = JsonConvert.DeserializeObject<API.Production_Countries>(response.Content);

                if (kind == "movie")
                {
                    details[0] = result.title.Trim();
                    details[1] = result.release_date.Trim();
                    details[2] = result.original_title.Trim();
                    details[3] = result.original_language.Trim();
                    details[4] = result.imdb_id.Trim();
                    details[5] = result.id.ToString().Trim();

                    /*Console.WriteLine($" - Title  : {details[0]}");
                    Console.WriteLine($" - Year   : {details[1]}");
                    Console.WriteLine($" - Orig   : {details[2]}");
                    Console.WriteLine($" - Lang   : {details[3]}");
                    Console.WriteLine($" - IMDB   : {details[4]}");
                    Console.WriteLine($" - TMDB   : {details[5]}");*/
                }
                else
                {
                    details[0] = result.name.Trim();
                    details[1] = result.first_air_date.Trim();
                    details[2] = result.original_name.Trim();
                    details[3] = result.original_language.Trim();
                    details[4] = String.Empty;
                    details[5] = result.id.ToString().Trim();

                    /*Console.WriteLine($" - Title  : {details[0]}");
                    Console.WriteLine($" - Year   : {details[1]}");
                    Console.WriteLine($" - Orig   : {details[2]}");
                    Console.WriteLine($" - Lang   : {details[3]}");
                    Console.WriteLine($" - IMDB   : Not Applicable");
                    Console.WriteLine($" - TMDB   : {details[5]}");*/
                }
                this.backDrop.Load($"https://image.tmdb.org/t/p/original{result.poster_path}");
                return details;
            }
            else
            {
                Console.WriteLine($"ERROR {response.StatusCode}");
                return null;
            }
        }
        public void Cuer()
        {
            try
            {
                foreach (string ln in File.ReadLines("Cue.txt"))
                {
                    this.vikiLink.Text = ln;
                    this.button2.PerformClick();
                }
            }
            catch
            {
                Log("Empty or Missing.");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show(e.CloseReason.ToString());
            try
            {
                Log("Quitting driver.");
                driver.Quit();
            }
            catch
            {
                //
            }
            Application.Exit();
        }
    }
}