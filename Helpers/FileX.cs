// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class FileX
    {
        /// <summary>
        /// Метод для получения размера файла
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <returns>Размер файла</returns>
        public static long GetInfoFile(string filename)
        {
            long getsize = 0;
            try
            {
                getsize = new FileInfo(filename).Length;
            }
            catch { }
            return getsize;
        }

        /// <summary>
        /// Метод для удаления .zip архива
        /// </summary>
        /// <param name="archivePath">Путь до архива</param>
        public static void DeleteArchive(string archivePath)
        {
            if (File.Exists(archivePath))
            {
                try
                {
                    File.Delete(archivePath);
                }
                catch { }
            }
        }

        /// <summary>
        /// Метод для удаления всех пустых папок и файлов
        /// </summary>
        /// <param name="dir">Путь до папки</param>
        public static void DeleteAllEmpty(string dir)
        {
            try
            {
                foreach (string d in Directory.EnumerateDirectories(dir)) { DeleteAllEmpty(d); }
                foreach (string f in Directory.EnumerateFiles(dir).Where(f => new FileInfo(f).Length == 0)) { File.Delete(f); }
                if (!Directory.EnumerateFileSystemEntries(dir).Any()) { Directory.Delete(dir); }
            }
            catch { }
        }

        /// <summary>
        /// Метод для получения MD5 хэш с файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5(string fileName)
        {
            string result = string.Empty;
            var Sb = new StringBuilder();
            try
            {
                using var md = MD5.Create();
                byte[] bytes = Encoding.ASCII.GetBytes(fileName), array = md.ComputeHash(bytes);

                for (int i = 0; i < array.Length; i++)
                {
                    Sb.Append(array[i].ToString("X2"));
                }
                result = Sb?.ToString().ToLower();
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Метод для получения полного пути процесса (WINAPI)
        /// </summary>
        /// <param name="process">Имя процесса</param>
        /// <param name="buffer">Размер буффера</param>
        /// <returns></returns>
        public static string GetMainModuleFileName(this Process process, int buffer = 1024)
        {
            string result = string.Empty;
            try
            {
                var fileNameBuilder = new StringBuilder(buffer);
                uint bufferLength = (uint)fileNameBuilder.Capacity + 1;
                result = NativeMethods.QueryFullProcessImageName(process.Handle, 0, fileNameBuilder, ref bufferLength) ? fileNameBuilder?.ToString() : null;
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Метод для чтения файла в памяти (массив байт)
        /// </summary>
        /// <param name="fileName">Путь до файла</param>
        /// <returns>Массив байтов данных файла</returns>
        public static byte[] CopyToMemory(string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                using var file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                file.CopyTo(ms);
            }
            catch { }
            return ms?.ToArray() ?? ms.GetBuffer() ?? null;
        }

        /// <summary>
        /// Метод для чтения всех байтов файла
        /// </summary>
        /// <param name="fileName">Путь до файла</param>
        /// <returns>Массив байтов файла</returns>
        public static byte[] ReadAllBytes(string fileName) 
        {
            byte[] readFully = null;
            try
            {
                readFully = File.ReadAllBytes(fileName);
            }
            catch { }
            return readFully;
        }

        /// <summary>
        /// Метод для чтения всех данных файла в потоке памяти
        /// </summary>
        /// <param name="fileName">Путь до файла</param>
        /// <returns>Массив байтов файла</returns>
        public static byte[] ReadAllToMemory(string fileName)
        {
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                byte[] file = File.ReadAllBytes(fileName);
                ms.Write(file, 0, file.Length);
            }
            catch { }
            return ms?.ToArray();
        }
    }
}