// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;
    using System.Security.Cryptography;

    public static class RndEx
    {
        public static string GenText
        {
            get
            {
                byte[] result = null;
                const int SALT = 10;
                try
                {
                    using var randomNumberGenerator = new RNGCryptoServiceProvider();
                    var randomNumber = new byte[SALT];
                    randomNumberGenerator.GetBytes(randomNumber);
                    result = randomNumber;
                }
                catch { }
                return Convert.ToBase64String(result).Replace("==", "").Replace("/", "").Replace("+", "");
            }
        }

        public static int Next(int min, int max)
        {
            try
            {
                using var rng = new RNGCryptoServiceProvider();
                var data = new byte[4];
                rng?.GetBytes(data);
                int generatedValue = Math.Abs(BitConverter.ToInt32(data, 0));
                int diff = max - min, mod = generatedValue % diff, normalizedNumber = min + mod;

                return normalizedNumber;
            }
            catch { }
            return 0;
        }
    }
}