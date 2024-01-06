// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;
    using System.IO;

    public static class Logger
    {
        /// <summary>
        /// <br>Метод для записи текста ошибок в .txt файл</br>
        /// <br>Method for writing error text to a .txt file</br>
        /// </summary>
        /// <param name="data"><br>Данные для записи</br><br>Data to write</br></param>
        public static void Write(string data)
        {
            if (Configurations.Act_LogClient)
            {
                try
                {
                    // Создаём файл (используем using чтобы автоматически закрывался
                    using var fileLog = File.AppendText(GlobalPaths.LogInizialize);
                    File.SetAttributes(GlobalPaths.LogInizialize, File.GetAttributes(GlobalPaths.LogInizialize) | FileAttributes.Hidden);
                    fileLog.WriteLine($"[{DateTime.Now:hh:mm:ss.fff tt}]:  {data}"); // Запись времени: текст
                    fileLog.Flush(); // Очистка буфера
                }
                catch { }
            }
        }
    }
}