using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viki
{
    internal static class Config
    {
        internal static bool Debug = false;
        internal static bool Feedback { get; set; }
        internal static bool DownloadSubtitles { get; set; } = true;
        internal static bool UseXMLTagsWhenAvailable { get; set; } = true;
        internal static string Key { get; set; } = String.Empty;
        internal static string ProcessLog { get; set; } = String.Empty;
        internal static string FilePath { get; set; } = String.Empty;
        internal static bool VikiIsLogged { get; set; }
        internal static bool GWVK { get; set; } = true;
        internal static bool ShowProcess_2 { get; set; }
        internal static bool DriverIsStarted { get; set; }
    }
}
