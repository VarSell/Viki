using System.IO;
using RestSharp;
namespace Viki
{
    internal static class Other
    {
        internal static string GenerateInitDataFromKeyId(string keyId)
        {
            // Generates the initialization data using the widevine system id
            List<byte> data = new List<byte>() { 0x00, 0x00, 0x00, 50, 112, 115, 115, 104, 0x00, 0x00, 0x00, 0x00, 237, 239, 139, 169, 121, 214, 74, 206, 163, 200, 39, 220, 213, 29, 33, 237, 0x00, 0x00, 0x00, 0x12, 0x12, 0x10 };
            data.AddRange(Convert.FromHexString(keyId.Replace("-", String.Empty)));
            return Convert.ToBase64String(data.ToArray());
        }
        internal static string ParseKeyIdFromManifest(string uri)
        {
            string keyId = String.Empty;

            RestClient restClient = new RestClient(uri);
            var fileBytes = restClient.DownloadData(new RestRequest("", Method.Get));
            string dest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump\manifest.mpd");
            File.WriteAllBytes(dest, fileBytes);
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
        internal static bool IsKeyValid(string keyPair)
        {
            if (String.IsNullOrEmpty(keyPair))
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
        internal static void DeleteTemporaryData()
        {
            foreach (string file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"src\dump")))
            {
                File.Delete(file);
            }
        }
    }
}
