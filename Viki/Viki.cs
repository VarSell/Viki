using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using static Viki.Other;
using static Viki.Config;

namespace Viki
{
    public class Viki
    {
        internal int Episodes { get; set; }
        internal int EpisodeNumber { get; set; }
        internal string Title { get; set; } = String.Empty;
        internal string Release { get; set; } = String.Empty;
        internal string Language { get; set; } = String.Empty;
        internal string TitleMeta { get; set; } = String.Empty;
        internal string ReleaseYear { get; set; } = String.Empty;
        internal string DecryptionKey { get; set; } = String.Empty;

        public void VParse(string url)
        {

            string html = GetSourceHTML(url);
            string sUrl = html.Split("<link rel=\"canonical\" href=")[1].Split("/>")[0].Replace("\"", String.Empty);
            this.Title = GetSafeFilename(html.Split("content=\"https://play.google.com/store/apps/details?id=com.viki.android\"/><title>")[1].Split("|")[0]);

            sUrl = GetSourceHTML(sUrl);

            string script = sUrl.Split("application/ld+json")[1].Split(">")[1].Split("</script")[0].Trim();
            var j = JsonConvert.DeserializeObject<VikiJson.Rootobject>(script);
            Console.WriteLine(j.url);
            this.Language = j.inLanguage;
            this.TitleMeta = j.alternativeHeadline[0];
            this.ReleaseYear = j.datePublished;
        }
        public void Decrypt(string encFile, string outFile)
        {
            Form1.SoftWare($" input=\"{encFile}\",stream=0,output=\"{outFile}\" --enable_raw_key_decryption --keys label=0:key_id={this.DecryptionKey.Split(":")[0]}:key={this.DecryptionKey.Split(":")[1]}", "tools\\shakapackager.exe");
        }
        /// <summary>
        /// Merges the given audio and video streams along with any .srt subtitles found in the \src\dump\ folder.
        /// </summary>
        public void Merge(string aFile, string vFile, string oFile)
        {
            string subArgs = string.Empty;
            if (DownloadSubtitles)
            {
                string ext = String.Empty;
                string[] subs = Directory.GetFiles($@"{AppDomain.CurrentDomain.BaseDirectory}src\dump\", "*.srt");

                foreach (string file in subs)
                {
                    string cult = Path.GetFileNameWithoutExtension(file).Split("]")[1].Replace(".", string.Empty);
                    // Anything less than 7kb is sure to be useless.
                    if (new FileInfo(file).Length > 7000)
                    {
                        string lang = Culture(cult);
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
            }

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
            ProcessLog = prc.StandardOutput.ReadToEnd();
            prc.WaitForExit();
        }
        /// <summary>
        /// Generates and writes track titles to the file given stream ID.
        /// </summary>
        public void Tag(string file, int streamID)
        {
            string args = String.Empty;

            if (streamID == 1)
            {
                // Video Stream ID
                string[] parts = new string[5];
                parts[0] = Culture(this.Language); // I'll leave this out from the track title
                parts[1] = MediaInfo($"\"{file}.mkv\" --Inform=Video;%BitRate%");
                parts[2] = MediaInfo($"\"{file}.mkv\" --Inform=Video;%FrameRate%");
                parts[3] = MediaInfo($"\"{file}.mkv\" --Inform=Video;%AspectRatio%");
                parts[4] = MediaInfo($"\"{file}.mkv\" --output");
                parts[4] = (parts[4].Replace(" ", String.Empty).Split("Formatprofile:")[1].Split("Formatsettings")[0]).Trim();
                args = String.Concat("MPEG-4 AVC / ", (Int32.Parse(parts[1]) / 1024).ToString(), " KiBps / ", parts[2], " fps / ", String.Concat(Math.Round(Convert.ToDecimal(parts[3]), 2).ToString(), ":1"), " / Profile ", parts[4]);
            }
            else if (streamID == 2)
            {
                // Audio Stream ID
                string[] parts = new string[6];
                parts[0] = Culture(this.Language);
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
                //parts[4] = MediaInfo($"\"{this.Release}.mkv\" --Inform=Audio;%BitDepth%"); // I don't want this for now.
                parts[5] = (Int32.Parse(MediaInfo($"\"dump//decAudio.mp4\" --Inform=General;%OverallBitRate%")) / 1024).ToString();
                args = String.Concat(parts[0], " / ", parts[1], " / ", parts[2], " / ", parts[3].Insert(2, "."), " kHz / ", parts[5] , " KiBps");
            }
            else
            {
                ProcessLog = $"Invalid stream ID '{streamID.ToString()}'.";
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
            ProcessLog = prc.StandardOutput.ReadToEnd();
            prc.WaitForExit();
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
        public string[] ParseSDStreamsLinks(string file)
        {
            string CleanLink(string line)
            {
                line = line.Substring(line.IndexOf("https"));
                line = line.Substring(0, line.IndexOf("<"));
                return line;
            }
            string[] tmp = new string[2];
            foreach (string line in File.ReadAllLines(file))
            {
                /*if (line.Contains("_dash_high_720p") && line.Contains("audio"))
                {
                    audioUrl = CleanLink(line);
                }*/
                if (line.Contains("_dash_high_1080p") && line.Contains("audio"))
                {
                    tmp[0] = CleanLink(line);
                }
                if (line.Contains("mov_drm") && line.Contains("audio"))
                {
                    tmp[0] = CleanLink(line);
                }
                // _dash_high_1080p_mov_drm
                /*if (line.Contains("_dash_high_720p") && line.Contains("video"))
                {
                    videoUrl = CleanLink(line);
                }*/
                if (line.Contains("_dash_high_1080p") && line.Contains("video"))
                {
                    tmp[1] = CleanLink(line);
                }
                if (line.Contains("mov_drm") && line.Contains("video"))
                {
                    tmp[2] = CleanLink(line);
                }
            }
            return tmp;
        }
        /// <summary>
        /// Starts '\src\tools\MediaInfo.exe' with the given arguments, and returns the output.
        /// </summary>
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
    }
}
