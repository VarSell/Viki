using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Viki
{
    internal static class Config
    {
        public const Boolean LOCAL_REQUEST = true;
        //private static readonly Byte[] data = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\data\config"));
        internal static Boolean Feedback { get; set; } = Properties.Settings.Default.Feedback;
        internal static Boolean DownloadSubtitles { get; set; } = Properties.Settings.Default.DownloadSubtitles;
        internal static Boolean UseXMLTagsWhenAvailable { get; set; } = Properties.Settings.Default.UseXMLTagsWhenAvailable;
        internal static Boolean Debug { get; set; } = Properties.Settings.Default.Debug;
        internal static Boolean Beta { get; set; } = Properties.Settings.Default.Beta;
        internal static String Key { get; set; } = String.Empty;
        internal static String ProcessLog { get; set; } = String.Empty;
        internal static String FilePath { get; set; } = String.Empty;
        internal static Boolean VikiIsLogged { get; set; }
        internal static Boolean SegmentedManifest { get; set; } = false;
        internal static Boolean ShowProcess_2 { get; set; }
        internal static Boolean DriverIsStarted { get; set; }
        internal static Boolean IsPremiumContent { get; set; } = false;
        internal static Boolean Aria2cVerbose { get; set; } = Properties.Settings.Default.Aria2cVerbose;
        internal static String UserEmail { get; set; } = String.Empty;
        internal static String UserPassword { get; set; } = String.Empty;
        internal static String CachedLicense { get; set; } = String.Empty;
    }
}
