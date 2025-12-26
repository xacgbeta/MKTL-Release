using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace MKTL.WPF.Services.Steam
{
    public class AccountInfo
    {
        public string SteamId { get; set; } = "";
        public string AccountName { get; set; } = "";
        public string PersonaName { get; set; } = "";
        public bool IsRecent { get; set; }
    }

    // 2. Add this Attribute to suppress the errors
    [SupportedOSPlatform("windows")]
    public class AccountService
    {
        private string GetSteamPath()
        {
            // Added null check to fix previous warnings too
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", "")?.ToString()?.Replace("/", "\\") ?? "";
        }

        public List<AccountInfo> GetAccounts()
        {
            var list = new List<AccountInfo>();
            string steamPath = GetSteamPath();
            if (string.IsNullOrEmpty(steamPath)) return list;

            string vdfPath = Path.Combine(steamPath, "config", ".vdf");
            if (!File.Exists(vdfPath)) return list;

            try
            {
                string content = File.ReadAllText(vdfPath);
                
                var matches = Regex.Matches(content, "\"(\\d{17})\"\\s*\\{([\\s\\S]*?)\\n\\s*}");
                
                foreach (Match m in matches)
                {
                    string block = m.Groups[2].Value;
                    string accName = Regex.Match(block, "\"AccountName\"\\s+\"([^\"]+)\"").Groups[1].Value;
                    string persona = Regex.Match(block, "\"PersonaName\"\\s+\"([^\"]+)\"").Groups[1].Value;
                    string recent = Regex.Match(block, "\"MostRecent\"\\s+\"([^\"]+)\"").Groups[1].Value;

                    list.Add(new AccountInfo 
                    { 
                        SteamId = m.Groups[1].Value, 
                        AccountName = accName, 
                        PersonaName = persona,
                        IsRecent = recent == "1"
                    });
                }
            }
            catch { /* Ignore read errors */ }

            return list;
        }

        public void SwitchAccount(string accountName)
        {
            try
            {
                // Kill Steam
                foreach (var proc in Process.GetProcessesByName("steam")) proc.Kill();
                foreach (var proc in Process.GetProcessesByName("steamwebhelper")) proc.Kill();
                foreach (var proc in Process.GetProcessesByName("steamservice")) proc.Kill();
                
                // Wait briefly for locks to release
                System.Threading.Thread.Sleep(2000);

                // Set Registry
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam", true);
                if (key != null)
                {
                    key.SetValue("AutoLoginUser", accountName);
                    key.SetValue("RememberPassword", 1);
                }
                
                // Restart Steam
                string exe = Path.Combine(GetSteamPath(), "steam.exe");
                if (File.Exists(exe))
                {
                    Process.Start(exe);
                }
            }
            catch
            {
                // Handle errors silently or log them
            }
        }
    }
}