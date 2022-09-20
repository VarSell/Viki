using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System.Net;
using static Viki.Config;

namespace Viki
{
    /// <summary>
    /// A repo of methods that are not tied to any specific service use.
    /// </summary>
    internal static class Other
    {
        /// <summary>
        /// Generates a Widevine PSSH (Initialization Data) from the passed KeyId.
        /// </summary>
        /// <param name="keyId">KID</param>
        /// <returns>A Widevine PSSH</returns>
        internal static string GenerateInitDataFromKeyId(string keyId)
        {
            // WIDEVINE UUID = EDEF8BA9-79D6-4ACE-A3C8-27DCD51D21ED , PLAYREADY UUID = 9A04F079-9840-4286-AB92-E65BE0885F95
            List<byte> data = new() { 0x00, 0x00, 0x00, 50, 112, 115, 115, 104, 0x00, 0x00, 0x00, 0x00, 237, 239, 139, 169, 121, 214, 74, 206, 163, 200, 39, 220, 213, 29, 33, 237, 0x00, 0x00, 0x00, 0x12, 0x12, 0x10 };
            data.AddRange(Convert.FromHexString(keyId.Replace("-", String.Empty)));
            return Convert.ToBase64String(data.ToArray());
        }

        /// <summary>
        /// Selects the correct decryption key (if any) from the passed array and starts decryption.
        /// </summary>
        /// <param name="encFile">CENC encrypted file</param>
        /// <param name="outFile">Output file</param>
        /// <param name="keys">Array of CENC decryption keys</param>
        /// <returns></returns>
        internal static bool DecryptWithKeyArray(string encFile, string outFile, string[] keys)
        {
            int c = 0;
            foreach (string key in keys)
            {
                ProcessLog = String.Empty;
                Form1.SoftWare($" input=\"{encFile}\",stream=0,output=\"{outFile}\" --enable_raw_key_decryption --keys label=0:key_id={key.Split(":")[0]}:key={key.Split(":")[1]}", "src\\tools\\shakapackager.exe");

                if (!ProcessLog.Contains("Cannot decrypt samples.") && !ProcessLog.Contains("Failed to parse")) // change this to match shaka failed output
                {
                    // Decryption did not see any errors.
                    MessageBox.Show(String.Concat("DECRYPTION_KEY SUCCESS ", c.ToString()));
                    break;
                }
                else
                {
                    MessageBox.Show(String.Concat("DECRYPTION_KEY FAILURE ", c.ToString()));
                }
            }
            return true;
        }
        /// <summary>
        /// Returns the parsed KeyId from a manifest url.
        /// </summary>
        /// <param name="uri">Manifest url</param>
        /// <returns>CENC KID</returns>
        internal static string ParseKeyIdFromManifest(string uri)
        {
            string keyId = String.Empty;

            RestClient restClient = new RestClient(uri);
            byte[] fileBytes = restClient.DownloadData(new RestRequest("", Method.Get)) ?? new byte[] { };
            string dest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump\manifest.mpd");
            File.WriteAllBytes(dest, fileBytes); // maybe convert bytes to str
            string[] content = File.ReadAllLines(dest);

            foreach (string ln in content)
            {
                if (ln.Contains("cenc:default_KID"))
                {
                    try
                    {
                        keyId = ln.Split("cenc:default_KID")[1].Split("\"")[1].Split("\"")[0].Trim();
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return keyId;
        }
        internal static string CalculateInitDataFromManifest(string uri)
        {
            return GenerateInitDataFromKeyId(ParseKeyIdFromManifest(uri));
        }
        /// <summary>
        /// Basic check for a valid structure.
        /// </summary>
        /// <param name="keyPair">KeyId:Key</param>
        /// <returns></returns>
        internal static bool IsKeyValid(string keyPair)
        {
            if (String.IsNullOrEmpty(keyPair))
            {
                return false;
            }
            else if (keyPair.Length != 65)
            {
                return false;
            }
            else
            {
                try
                {
                    string kid = keyPair.Split(":")[0];
                    string key = keyPair.Split(":")[1];

                    if ((kid.Length != 32) && key.Length != 32)
                    {
                        return false;
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Returns source HTML for given url.
        /// </summary>
        public static string GetSourceHTML(String url)
        {
            using (RestClient client = new RestClient(url))
            {
                RestResponse response = client.Execute(new RestRequest());
                return response.Content;
            }
        }
        /// <summary>
        /// Returns in Int64 the 'Content-Length' header after opening the url for reading.
        /// </summary>
        internal static long DetermineByteSize(string url)
        {
            try
            {
                using (WebClient client = new System.Net.WebClient())
                {
                    client.OpenRead(url);
                    Int64 bytes_total = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                    return bytes_total;
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Returns a ChromeDriver with custom settings.
        /// </summary>
        internal static ChromeDriver GetDriver(bool anon = false)
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
            options.AddArgument("--window-size=0,0");
            options.AddArgument("no-sandbox");
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            options.LeaveBrowserRunning = true;

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\tools\"));
            chromeDriverService.HideCommandPromptWindow = true;
            DriverIsStarted = true;
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
        /// <summary>
        /// Takes a two-letter ISO 639-1 code and returns it's Display Name in English.
        /// </summary>
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
        /// <summary>
        /// Monitors file growth and reports to progress bar.
        /// </summary>
        /// <param name="file">Filepath to monitor</param>
        /// <param name="size">Number of bytes</param>
        /// <param name="prg">Progress Bar</param>
        internal static async Task Monitor(string file, double size, ProgressBar prg)
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
                    prg.Value = (int)percentage;
                    if (percentage == 100)
                        return;
                    Thread.Sleep(700);
                }
            });
        }
        /// <summary>
        /// Filters a YT-DLP like output for the percentage, and set's the set progressbar value to it.
        /// </summary>
        internal static async Task GetAndSetPercentageFromExternalDownloader(string ln, ProgressBar prg)
        {
            await Task.Run(() =>
            {
                int rCount = 0;
            retry:
                try
                {
                    decimal p = Convert.ToInt32(ln.Split("[download]")[1].Split("of")[0]);
                    int value = (int)Math.Round(d: p);
                    prg.Value = value;
                    if (value == 100)
                        return;
                }
                catch
                {
                    rCount++;
                    if (rCount == 12)
                        return;
                    goto retry;
                }
            });
        }
        /// <summary>
        /// Filters a string for potentially invalid characters.
        /// </summary>
        /// <returns>A safe filename.</returns>
        internal static string GetSafeFilename(string filename)
        {
            return string.Join(String.Empty, filename.Split(Path.GetInvalidFileNameChars())).Trim();
        }
        internal static void DeleteTemporaryData()
        {
            foreach (string file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump")))
            {
                File.Delete(file);
            }
        }
    }
}
