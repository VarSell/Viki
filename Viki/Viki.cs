using RestSharp;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace Viki
{
    public class Viki
    {
        internal int Episodes { get; set; }
        internal int EpisodeNumber { get; set; }
        internal string Title { get; set; }
        internal string Release { get; set; }
        internal string Language { get; set; }
        internal string TitleMeta { get; set; }
        internal string ReleaseYear { get; set; }


        public void VParse(string url)
        {

            string html = Source(url);

            string sUrl = html.Split("<link rel=\"canonical\" href=")[1].Split("/>")[0].Replace("\"", String.Empty);

            this.Title = GetSafeFilename(html.Split("content=\"https://play.google.com/store/apps/details?id=com.viki.android\"/><title>")[1].Split("|")[0]);

            sUrl = Source(sUrl);

            string script = sUrl.Split("application/ld+json")[1].Split(">")[1].Split("</script")[0].Trim();
            //Console.WriteLine(script);
            var j = JsonConvert.DeserializeObject<Json.Rootobject>(script);
            Console.WriteLine(j.url);
            this.Language = j.inLanguage;
            //Console.WriteLine(j.name);
            this.TitleMeta = j.alternativeHeadline[0];
            this.ReleaseYear = j.datePublished;
        }
        public void Decrypt(string encFile, string outFile, string key)
        {
            Form1.SoftWare($" input=\"{encFile}\",stream=0,output=\"{outFile}\" --enable_raw_key_decryption --keys label=0:key_id={key.Split(":")[0]}:key={key.Split(":")[1]}", "tools\\shakapackager.exe");
        }
        public void Merge(string aFile, string vFile, string oFile)
        {
            // sort the subtitles
            string[] subs = Directory.GetFiles($@"{AppDomain.CurrentDomain.BaseDirectory}src\dump\", "*.srt");
            string subArgs = string.Empty;

            foreach (string file in subs)
            {
                string cult = ((Path.GetFileNameWithoutExtension(file)).Split("]")[1].Replace(".", string.Empty));
                if (new FileInfo(file).Length > 7000)
                {
                    string lang = Form1.Culture(cult);
                    string subDef = "no";
                    if (cult == "en")
                    {
                        subDef = "yes";
                    }
                    if (lang == "und"  || String.IsNullOrEmpty(lang))
                    {
                        subArgs += $" --track-name \"0:{cult}\" --language 0:und --compression 0:none --default-track 0:{subDef} \"{file}\"";
                    }
                    else
                    {
                        subArgs += $" --track-name \"0:{lang}\" --language 0:{cult} --compression 0:none --default-track 0:{subDef} \"{file}\"";
                    }
                }
            }
            //("-o $var$release.mkv$var --language 0:$flang --compression 0:none --default-track 0:yes .\src\temp\processing.mkv $audio_merge $chapters $tags --title $var$header$var $paths") ----------     {/*this.Tags*/} right after aFile
            string a = $"-o \"{Path.Combine(oFile, this.Release)}.mkv\" --language 0:ko --compression 0:none --default-track 0:yes \"{vFile}\" --language 0:ko --compression 0:none --default-track 0:yes \"{aFile}\"  --title \"{this.TitleMeta}\" {subArgs}";

            string tagDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump\metadata.xml");
            if (File.Exists(tagDir))
            {
                a += $" --global-tags \"{tagDir}\"";
            }

            var prc = new Process();
            prc.StartInfo.FileName = @"tools\mkvmerge.exe";
            prc.StartInfo.Arguments = a;
            prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            prc.StartInfo.CreateNoWindow = true;
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.RedirectStandardOutput = true;
            prc.Start();
            Form1.procLog = prc.StandardOutput.ReadToEnd();
            prc.WaitForExit();
        }
        public void Tag(string file, int streamID)
        {
            string args = String.Empty;

            if (streamID == 1)
            {
                string[] parts = new string[5];
                parts[0] = Form1.Culture(this.Language); // not needed
                parts[1] = MediaInfo($"\"{file}.mkv\" --Inform=Video;%BitRate%");
                parts[2] = MediaInfo($"\"{file}.mkv\" --Inform=Video;%FrameRate%");
                parts[3] = MediaInfo($"\"{file}.mkv\" --Inform=Video;%AspectRatio%");
                parts[4] = MediaInfo($"\"{file}.mkv\" --output");
                parts[4] = (parts[4].Replace(" ", String.Empty).Split("Formatprofile:")[1].Split("Formatsettings")[0]).Trim();
                args = String.Concat("MPEG-4 AVC / ", (Int32.Parse(parts[1]) / 1024).ToString(), " KiBps / ", parts[2], " fps / ", String.Concat(Math.Round(Convert.ToDecimal(parts[3]), 2).ToString(), ":1"), " / Profile ", parts[4]);
            }
            else if (streamID == 2)
            {
                //Japanese / AAC Audio / 2.0 / 48 kHz / 192 kbps
                string[] parts = new string[6];
                parts[0] = Form1.Culture(this.Language);
                parts[1] = MediaInfo($"\"{file}.mkv\" --output").Split("Audio")[1].Split("2")[1].Split(":")[1].Split("F")[0].Trim();
                parts[2] = MediaInfo($"\"{file}.mkv\" --Inform=Audio;%Channel(s)%");
                switch (Int32.Parse(parts[2]))
                {
                    case 1:
                        parts[2] = "1.0";
                        break;
                    case 2:
                        parts[2] = "2.0";
                        break;
                    case 6:
                        parts[2] = "5.1";
                        break;
                    case 8:
                        parts[2] = "7.1";
                        break;
                    default:
                        MessageBox.Show(String.Format("Invalid Channel(s): '{0}'.", parts[2]));
                        break;
                }
                parts[3] = MediaInfo($"\"{file}.mkv\" --Inform=Audio;%SamplingRate%");
                //parts[4] = MediaInfo($"\"{this.Release}.mkv\" --Inform=Audio;%BitDepth%");
                parts[5] = (Int32.Parse(MediaInfo($"\"dump//decAudio.mp4\" --Inform=General;%OverallBitRate%")) / 1024).ToString();
                args = String.Concat(parts[0], " / ", parts[1], " / ", parts[2], " / ", parts[3].Insert(2, "."), " kHz / ", parts[5] , " KiBps");
            }
            else
            {
                Form1.procLog = $"Invalid stream ID '{streamID.ToString()}'.";
            }

            args = $"\"{file}.mkv\" --edit track:{streamID} --set name=\"{args}\"";

            var prc = new Process();
            prc.StartInfo.FileName = @"tools\mkvpropedit.exe";
            prc.StartInfo.Arguments = args;
            prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.CreateNoWindow = true;
            prc.StartInfo.RedirectStandardOutput = true;
            prc.Start();
            Form1.procLog = prc.StandardOutput.ReadToEnd();
            prc.WaitForExit();
        }
        public static string MediaInfo(string args, bool std = false)
        {
            Process SoftWare = new Process();
            SoftWare.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            SoftWare.StartInfo.FileName = "tools\\mediainfo.exe";
            SoftWare.StartInfo.Arguments = args;
            SoftWare.StartInfo.CreateNoWindow = true;
            if (std == true)
            {
                SoftWare.StartInfo.RedirectStandardError = true;
                SoftWare.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
                SoftWare.Start();

                using (StreamReader reader = SoftWare.StandardError)
                {
                    string result = reader.ReadToEnd();
                    SoftWare.WaitForExit();
                    return result.Trim();
                }
            }
            else
            {
                SoftWare.StartInfo.RedirectStandardOutput = true;
                SoftWare.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                SoftWare.Start();

                using (StreamReader reader = SoftWare.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    SoftWare.WaitForExit();
                    return result.Trim();
                }
            }
        }
        internal static string GetSafeFilename(string filename)
        {
            return string.Join(String.Empty, filename.Split(Path.GetInvalidFileNameChars())).Trim();
        }
        public static string Source(String url)
        {
            var client = new RestClient(url);
            var request = new RestRequest();
            var response = client.Execute(request);
            return response.Content;
        }
    }
}
