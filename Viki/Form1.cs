using OpenQA.Selenium;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V85.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V85.Network;
using Newtonsoft.Json;
using System.Diagnostics;
using RestSharp;
using System.Net;
using System.IO;
using static Viki.Other;
using static Viki.Config;

namespace Viki
{
    public partial class Form1 : Form
    {
        internal static IWebDriver driver;

        public Viki v = new Viki();
        public Form1()
        {
            InitializeComponent();
        }
        /*public void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            _=SendBugReport(e.Exception, new StackTrace(true));
        }

        public void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _=SendBugReport(e.ExceptionObject as Exception, new StackTrace(true));
        }*/

        /// <summary>
        /// Downloads the passed url and reports the progress to a progressBar.
        /// </summary>
        public void Download(string url, string outFile)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                #pragma warning disable CS4014
                // The following call is not meant to be awaited
                Monitor(file: outFile, size:Convert.ToDouble(DetermineByteSize(url)), prg:progressBar1);
                #pragma warning restore CS4014
                client.DownloadFile(url, outFile);
            }
        }
        /// <summary>
        /// Logs into https://www.viki.com with the given driver if credentials are set. Skips login if not.
        /// </summary>
        public void Login(IWebDriver driver)
        {
            if (VikiIsLogged)
            {
                Log("Logged in.");
                LoadPage(driver, this.vikiLink.Text.Trim());
                return;
            }
            string[] credentials = File.ReadAllLines("credentials.txt");
            try
            {
                string email = credentials[0];
                string passwd = credentials[1];
                if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(passwd))
                {
                    throw new IndexOutOfRangeException();
                }
                Log("Logging in.");
                LoadPage(driver, $"https://www.viki.com/sign-in?return_to={this.vikiLink.Text.Trim()}?auto_play=1");
                string[] t = { email, passwd };
                int i = 0;
                foreach (IWebElement c in driver.FindElements(By.CssSelector(".sc-dtDOqo.gELvNZ")))
                {
                    c.SendKeys(t[i]);
                    i++;
                }
                driver.FindElements(By.CssSelector(".sc-dtDOqo.gELvNZ"))[1].SendKeys(OpenQA.Selenium.Keys.Return);
                VikiIsLogged = true;
            }
            catch (IndexOutOfRangeException)
            {
                Log("Skipping Login.");
                LoadPage(driver, this.vikiLink.Text);
            }
        }
        public void LoadPage(IWebDriver driver, string uri)
        {
            driver.Navigate().GoToUrl(uri);
        }
        [STAThread] // doesn't do shit
        private async void button1_Click(object sender, EventArgs e)
        {
            Log("[D] TestMessage.");
            Log("Normal Message.");
            return;
            string DECRYPTION_KEY_TRUE = "5e1f3a95220e4f43b5bbbf5e6a235552:fdd89d74237b53a5a4906ed65a41ae4b";
            string DECRYPTION_KEY_FALSE_0 = "511f3a95220e4f43b5bbbf5e6a235552:fdd89d74237b53a5a4906ed65a41ae4b";
            string DECRYPTION_KEY_FALSE_1 = "521f3a95220e4f43b5bbbf5e6a235552:fdd89d74237b53a5a4906ed65a41ae4b";
            string[] DECRYPTION_ARRAY = { DECRYPTION_KEY_FALSE_1, DECRYPTION_KEY_TRUE, DECRYPTION_KEY_FALSE_0 };
            string inFile = @"C:\Users\User\source\repos\Viki\Viki\bin\Debug\net6.0-windows\src\encVideo.mp4";
            string oFile = @"C:\Users\User\source\repos\Viki\Viki\bin\Debug\net6.0-windows\src\encVideo.mp4";

            DecryptWithKeyArray(inFile, oFile, DECRYPTION_ARRAY);
            //Log("<message>", debug: Config.Debug);
            /*string file = @"C:\Users\User\Desktop\Files.Bak.txt";
            byte[] b = { 0x0, 0x1, 0x0, 0x1 };
            byte[] b = { 0x0, 0x1, 0x0, 0x1 };
            File.WriteAllBytes(file, b);
            foreach (byte b2 in File.ReadAllBytes(file))
            {
                Log(Convert.ToBoolean(b2).ToString());
            }*/
            //consoleControl1.StartProcess("yt-dlp", "\"https://manifest-kcp.viki.io/v1/1153146v/limelight/main/mpd/normal/kcp/high/hd/kcp/dt2_dt3/manifest.mpd?h_country=us&h_data_center=veg-2&h_host=playback-streams-68f9bdbb76-rh49b&h_timestamp=1661483431&tag=mpdhd%3Ahigh%3Akcp%3AsourceKCP%3Ampd%3Auncached%3Adt2_dt3&app=100000a&u=64977021u&sig=007214727355672d4e896b1dd72a44cdfb9ef939\" --allow-u -f ba");


            //// new idea for showing Plus download status. YT-DLP and ARIA2C cause problems for rtf boxes, so maybe a minimalistic output
            /// like AXEL, which could report only simple percentage could be optimal. In fact, I can even plug it into the prg bar, with
            /// no need to print as text. I just need to output only as percentage / numeric progress, then Int32.Parse() e.Data and set.
            /// Thanks to Thomas Arch on TG for the news that this download accelerator even exists.
            /// 
            /// Now I just need to edit LoadViki() to add support for detecting Plus / Segmented streams.
            /// fuck axel

            return;
            await Task.Run(() =>
            {
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads"));
                }
                catch (Exception ex)
                {
                    Record.SendBugReport(ex, new StackTrace(true), LogBox);
                }
            });
        }
        public void LoadViki()
        {
            this.keyPair.Text = String.Empty;
            string uri = this.vikiLink.Text;
            Login(driver);
            Log("Parsing metadata.");
            v.VParse(uri);

        retry:

            try
            {
                driver.FindElement(By.Id("html5_player_id")).Click();
            }
            catch
            {
                goto retry;
            }

            Log("Scanning network.");
            string[] links = GetLinksFromDriver(driver);

            // Look here to check if link is Plus

            Log("Terminating tab.");
            ResetPage(driver);
            Log("Downloading manifest.");

            RestClient client = new RestClient(links[0]);
            RestRequest request = new RestRequest();
            RestResponse response = client.Execute(request);

            File.WriteAllText(@"dump\_dump", response.Content);

            string[] _temp = File.ReadAllLines(@"dump\_dump");
            string[] streams = new string[2];
            bool highFound = false;

            foreach (string ln in _temp)
            {
                if (ln.Contains("high") && ln.Contains(".mpd"))
                {
                    highFound = true;
                    Log("Found high manifest.");
                    string l = ln.Split("<BaseURL>")[1].Split("</BaseURL>")[0];
                    using (WebClient client1 = new System.Net.WebClient())
                    {
                        Stream stream = client1.OpenRead(l);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        streams = v.ParseStreamsLinks(content.Split("\n"));
                    }
                }
            }
            if (!highFound)
            {
                Log("No high found.");
                streams = v.ParseSDStreamsLinks(@"dump\_dump");
            }
            
            Log($"Audio Filesize: {(DetermineByteSize(streams[0]) / 1024 / 1024).ToString()} MiB");
            Log($"Video Filesize: {(DetermineByteSize(streams[1]) / 1024 / 1024).ToString()} MiB");

            this.audioUrl.Text = streams[0];
            this.videoUrl.Text = streams[1];

            Log("Requesting keys.");
            if (!DownloadSubtitles)
            {
                Log("Subtitles disabled.");
            }
            else
            {
                _=ScanSubs();
            }
            if (!GWVK)
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

            this.releaseName.Text = String.Concat(v.Title.Trim());
            this.headerMeta.Text = $"{v.TitleMeta} ({DateTime.Parse(v.ReleaseYear).Year.ToString()})";

            TMDBAPIRequest(true);

            this.languageISO.Text = v.Language;
            this.releaseYear.Text = v.ReleaseYear;
        }
        /// <summary>
        /// Performs an API call to TheMovieDataBase.
        /// </summary>
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
                    // was for Mubi
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

        /// <summary>
        /// Prints the given string to the RichTextBox.
        /// If the string contains '[S]' it will be printed as strikethrough.
        /// </summary>
        public void Log(string alert, int id = 0)
        {
            if (alert.Contains("[D]"))
            {
                if (Config.Debug)
                {
                    Log(alert.Replace("[D]", String.Empty).Trim());
                    return;
                }
                else
                {
                    return;
                }
            }
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
        /// <summary>
        /// Starts the given executable (default=python) with the given arguments.
        /// Redirects the output / Received Data to the variable 'procLog' and sends it to Log() after completion.
        /// </summary>
        public static void SoftWare(string args, string executable = "python.exe")
        {
            Config.ProcessLog = string.Empty;
            Process proc = new Process()
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
                ProcessLog += e.Data;
            }
        }
        public void SoftWare_2(string args)
        {
            Process proc_2 = new Process()
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
            if (e.Data != null)
            {
                if (LogBox.Text.Length > 7000)
                {
                    LogBox.Clear();
                }
                if (ShowProcess_2)
                {
                    Log(e.Data);
                }
            }

        }
        /// <summary>
        /// Loops (using a generic filter) through performance logs from given driver until the following conditions are met:
        /// 1; Manifest url is found.  2; License url is found.
        /// Should be called right after starting the stream.
        /// </summary>
        public string[] GetLinksFromDriver(IWebDriver driver)
        {
            string mpd = String.Empty;
            string lic = String.Empty;
            bool foundMPD = false;
            bool foundLIC = false;

            while (!foundMPD || !foundLIC)
            {
                foreach (LogEntry? log in driver.Manage().Logs.GetLog("performance"))
                {
                    try
                    {
                        if (log.ToString().Contains(".mpd") && foundMPD == false)
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
                    catch
                    {

                    }
                }
            }

            return new string[] { mpd, lic };
        }
        public void ResetPage(OpenQA.Selenium.IWebDriver drv)
        {
            drv.Navigate().GoToUrl("about:blank");
        }
        /*
        internal string GetPSSHFromFragment(string manifest)
        {
            Log("Downloading fragment.");
            SoftWare_2($"yt-dlp --test --allow-u -f bv -o \"dump\\fragment.mp4\" \"{manifest}\"");
            Log("Retrieving initData.");
            SoftWare(@"scripts\PSSHFromFrag.py");
            File.Delete(@"dump\fragment.mp4");
            return Clipboard.GetText();
        }*/
        /// <summary>
        /// Generates a PSSH (initialization data) using the widevine UUID.
        /// </summary>
        /// <param name="KID">KeyId</param>
        /// <returns>A widevine PSSH</returns>
        internal string GetInitDataFromKID(string KID)
        {
            List<byte> data = new List<byte>() { 0x00, 0x00, 0x00, 50, 112, 115, 115, 104, 0x00, 0x00, 0x00, 0x00, 237, 239, 139, 169, 121, 214, 74, 206, 163, 200, 39, 220, 213, 29, 33, 237, 0x00, 0x00, 0x00, 0x12, 0x12, 0x10 };
            data.AddRange(Convert.FromHexString(KID.Replace("-", "")));
            return Convert.ToBase64String(data.ToArray());
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Directory.SetCurrentDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src"));
            if (!DriverIsStarted)
            {
                Log("Starting driver.");
                driver = GetDriver();
            }
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
                    foreach (string file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump")))
                    {
                        File.Delete(file);
                    }
                    LoadViki();
                    _=EnableButtons(false);
                });

                // Thread needs to be static for clipboard access.
                this.keyPair.Text = Clipboard.GetText().Trim();
                if (!KeyValidation(this.keyPair.Text))
                {
                    Log("Invalid DecryptionKey, no point in continuing.");
                }
                v.DecryptionKey = this.keyPair.Text;

                await Task.Run(() =>
                {
                    Log("Downloading Audio");
                    Download(this.audioUrl.Text, @"dump\encAudio.mp4");
                    Log("Downloading Video");
                    Download(this.videoUrl.Text, @"dump\encVideo.mp4");

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
                    { 
                        // Do nothing.
                    }

                    FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", folder);
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }

                    v.Release = this.releaseName.Text;
                    v.TitleMeta = this.headerMeta.Text;

                    if (UseXMLTagsWhenAvailable)
                    {
                        LoadXmlTags();
                    }
                    Log("Decrypting Audio.");
                    v.Decrypt(encFile:@"dump\encAudio.mp4", outFile:@"dump\decAudio.mp4");
                    Log(ProcessLog);
                    Log("Decrypting Video");
                    v.Decrypt(encFile: @"dump\encVideo.mp4", outFile:@"dump\decVideo.mp4");
                    Log(ProcessLog);
                    Log("Merging.");
                    v.Merge(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump\decAudio.mp4"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump\decVideo.mp4"), FilePath);
                    Log(ProcessLog);

                    Log("Tagging metadata.");
                    v.Tag(Path.Combine(FilePath, this.releaseName.Text), 1);
                    v.Tag(Path.Combine(FilePath, this.releaseName.Text), 2);

                    Log(ProcessLog);
                    Log("Cleaning temp.");
                    Other.DeleteTemporaryData();
                    Log("Completed.");
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
            string subSet = "--subs-format \"srt\"";
            SoftWare_2($"yt-dlp --cookies \"iqiyiCookies.txt\" --all-subs {subSet} --remux-video \"mkv\" --embed-subs --clean {this.vikiLink.Text}");
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            //DecryptWithKeyArray()
            //Log(GenerateInitDataFromKeyId("87fddefed5bb539ab11a478d756ffc68"));
            //Log(GenerateInitDataFromKeyId("35cfc571679545829b322475ac8af898"));
            Form2 f = new Form2();
            f.Show();
        }
        public bool KeyValidation(string fullKey)
        {
            if (String.IsNullOrEmpty(fullKey))
            {
                Log("Failed Data Retrieval.");
                return false;
            }
            else
            {
                if (fullKey.Length != 65)
                {
                    return false;
                }
                try
                {
                    string kid = fullKey.Split(":")[0];
                    string key = fullKey.Split(":")[1];

                    if ((kid.Length != 32) && key.Length != 32)
                    {
                        Log("Incorrect key Length");
                        return false;
                    }
                    return true;
                }
                catch
                {
                    Log("Incorrect key format.");
                    return false;
                }
            }

        }
        /// <summary>
        /// Downloads all available subtitles to '\src\dump\' with the format 'srt' preferred.
        /// Prints filenames using Log() after the downloads finish.
        /// </summary>
        public async Task ScanSubs()
        {
            await Task.Run(() =>
            {
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
                this.button2.Enabled = enabled;
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
            /*if (this.releaseName.Text.Contains("Episode"))
            {
                //
            }*/
        }
        /// <summary>
        /// Checks to see whether the PC is connected to a network.
        /// </summary>
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
        public string TMDB_shifter(string search, string kind)
        {
            string url = string.Concat("https://www.themoviedb.org/search?query=", search);
            string html = GetSourceHTML(url);
            string[] match = html.Split(new string[] { $"class=\"result\" href=\"/{kind}/" }, StringSplitOptions.RemoveEmptyEntries);
            match[1] = match[1].Substring(0, match[1].IndexOf("\"") + 1);
            match[1] = match[1].Remove(match[1].Length - 1).Trim();
            Log($"TMDB ID: {match[1]}");
            return match[1];
        }
        public string[] TMDBRequest(string kind, string id)
        {
            string url = $"https://api.themoviedb.org/3/{kind}/{id}?api_key=dc0141096c9d0bb09bdde4025387888b";
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest();
            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string[] details = new string[7];
                TheMovieDataBaseAPIResponse.Rootobject result = JsonConvert.DeserializeObject<TheMovieDataBaseAPIResponse.Rootobject>(response.Content);
                TheMovieDataBaseAPIResponse.Production_Countries P_C = JsonConvert.DeserializeObject<TheMovieDataBaseAPIResponse.Production_Countries>(response.Content);
                
                if (kind == "movie")
                {
                    details[0] = result.title.Trim();
                    details[1] = result.release_date.Trim();
                    details[2] = result.original_title.Trim();
                    details[3] = result.original_language.Trim();
                    details[4] = result.imdb_id.Trim();
                    details[5] = result.id.ToString().Trim();
                }
                else
                {
                    details[0] = result.name.Trim();
                    details[1] = result.first_air_date.Trim();
                    details[2] = result.original_name.Trim();
                    details[3] = result.original_language.Trim();
                    details[4] = String.Empty;
                    details[5] = result.id.ToString().Trim();
                }
                this.backDrop.Load($"https://image.tmdb.org/t/p/original{result.poster_path}");
                return details;
            }
            else
            {
                Log($"ERROR {response.StatusCode}");
                return null;
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
                // Driver is already closed.
            }
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.vikiLink.Text = "https://www.viki.com/videos/1115053v";
            Log(Environment.MachineName);
            Log($"Version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            //Log(Environment.OSVersion.ToString());
            Log($"Runtime {Environment.Version.ToString()}");
            Log("Application started.");
        }
    }
}