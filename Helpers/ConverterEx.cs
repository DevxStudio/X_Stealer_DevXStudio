// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class ConverterEx
    {
        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод соединяющий массив байтов</br>
        /// <br><b>~[ENG]~</b></br><br>Method concatenating an array of bytes</br>
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public static byte[] Concat(params byte[][] arrays) => arrays?.SelectMany(x => x)?.ToArray();

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для преобразования строки в строку UTF8</br>
        /// <br><b>~[ENG]~</b></br><br>Method to convert string to UTF8 string</br>
        /// </summary>
        /// <param name="text"><br><b>~[RUS]~</b></br><br>Строка для преобразования</br><br><b>~[ENG]~</b></br><br>String to convert</br></param>
        /// <returns><br><b>~[RUS]~</b></br><br>Преобразованная строка в UTF8</br><br><b>~[ENG]~</b></br><br>Converted string to UTF8</br></returns>
        public static string StringToUTF8(string text)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    byte[] bData = Encoding.Default.GetBytes(text);
                    result = Encoding.UTF8.GetString(bData);
                }
                catch { return text; }
            }
            return result;
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для преобразования текста в массив байтов.</br>
        /// <br><b>~[ENG]~</b></br><br>Method for converting text to a byte array.</br>
        /// </summary>
        /// <param name="defaultEnc">true - Default, false - UTF8</param>
        /// <param name="text"><br><b>~[RUS]~</b></br><br>Строка для преобразования</br><br><b>~[ENG]~</b></br><br>String to convert</br></param>
        /// <returns><br><b>~[RUS]~</b></br><br>Преобразованная строка в массив байтов</br><br><b>~[ENG]~</b></br><br>Converted string to byte array</br></returns>
        public static byte[] ToBytes(bool defaultEnc, string text)
        {
            byte[] result = null;
            try
            {
                result = defaultEnc ? Encoding.Default.GetBytes(text) : Encoding.UTF8.GetBytes(text);
            }
            catch { }
            return result;
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для преобразования байтового массива в строку.</br>
        /// <br><b>~[ENG]~</b></br><br>Method for converting byte array to string.</br>
        /// </summary>
        /// <param name="defaultStr">true - Default, false - UTF8</param>
        /// <param name="massive"><br><b>~[RUS]~</b></br><br>Массив для преобразования</br><br><b>~[ENG]~</b></br><br>Array to convert</br></param>
        /// <returns><br><b>~[RUS]~</b></br><br>Преобразованная строка</br><br><b>~[ENG]~</b></br><br>The converted string</br></returns>
        public static string ToString(bool defaultStr, byte[] massive)
        {
            string result = string.Empty;
            if (massive != null && massive.Any())
            {
                try
                {
                    result = defaultStr ? Encoding.Default.GetString(massive) : Encoding.UTF8.GetString(massive);
                }
                catch { }
            }
            return result;
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для удаление пустых байтов.</br>
        /// <br><b>~[ENG]~</b></br><br>Method for removing empty bytes.</br>
        /// </summary>
        /// <param name="array"><br><b>~[RUS]~</b></br><br>Массив байтов</br><br><b>~[ENG]~</b></br><br>Byte array</br></param>
        /// <returns><br><b>~[RUS]~</b></br><br>Массив байтов без пустых нулей</br><br><b>~[ENG]~</b></br><br>Array of bytes without empty zeros</br></returns>
        public static byte[] TrimEnd(byte[] array)
        {
            if (array is not null)
            {
                int lastIndex = Array.FindLastIndex(array, b => b != 0);
                Array.Resize(ref array, lastIndex + 1);
                return array;
            }
            throw new ArgumentNullException(nameof(array));
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Преобразование ключа из БД в байтовый массив</br>
        /// <br><b>~[ENG]~</b></br><br>Converting a key from a DB to a byte array</br>
        /// </summary>
        /// <param name="hex"><br><b>~[RUS]~</b></br><br>Строка для преобразования</br><br><b>~[ENG]~</b></br><br>String to convert</br></param>
        /// <returns><br><b>~[RUS]~</b></br><br>Преобразованная строка в массив байтов</br><br><b>~[ENG]~</b></br><br>Converted string to byte array</br></returns>
        public static byte[] StringToByteArray(string hex) => hex.Length % 2 != 0 ? throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hex)) : (Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16))?.ToArray());

        /// <summary>
        /// Метод для преобразования объекта в строку
        /// </summary>
        /// <param name="value">Объект для преобразования</param>
        /// <param name="mb">Конвертировать в Megabytes</param>
        /// <returns></returns>
        public static string ConverterUnit(object value, bool mb)
        {
            string result = string.Empty;
            try
            {
                result = Convert.ToString(Math.Round(mb ? Convert.ToDouble(value) / 0x4000_0000 * 0x3E8 : Convert.ToDouble(value) / 0x4000_0000, 0x2));
            }
            catch { }
            return result;
        }
    }
}