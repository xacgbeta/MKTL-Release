using System.Diagnostics;
using System.IO;

namespace MKTL.WPF.Helpers
{
    public static class ProcessHelper
    {
        public static void KillSteam()
        {
            string[] processes = { "steam", "steamwebhelper", "steamservice" };
            foreach (var procName in processes)
            {
                foreach (var proc in Process.GetProcessesByName(procName))
                {
                    try { proc.Kill(); } catch { }
                }
            }
        }

        public static void StartProcess(string path, string args = "")
        {
            if (File.Exists(path))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = args,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(path)
                });
            }
        }

        public static void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}   