using Scrypt;
using System.IO;
using System.Text.RegularExpressions;
using MKTL.WPF.Services.Steam;

namespace MKTL.WPF.Services.Steam
{
    public class PinRecoveryService
    {
        private readonly AccountService _accountService;

        public PinRecoveryService()
        {
            _accountService = new AccountService();
        }

        public async Task<string> RecoverPinAsync(IProgress<string> progress)
        {
            string steamPath = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", "")?.ToString()?.Replace("/", "\\");
            if (steamPath == null) return "Error: Steam not found";
            
            string userDataPath = Path.Combine(steamPath, "userdata");
            string? userDir = Directory.GetDirectories(userDataPath).FirstOrDefault();
            if (userDir == null) return "Error: No user data found";

            string vdfPath = Path.Combine(userDir, "config", ".vdf");
            if (!File.Exists(vdfPath)) return "Error: .vdf not found";

            // 2. Parse VDF for Salt and Hash
            string content = File.ReadAllText(vdfPath);
            var match = Regex.Match(content, "\"ParentalSettings\"\\s*\\{[\\s\\S]*?\"settings\"\\s*\"([0-9A-Fa-f]+)\"");
            
            if (!match.Success) return "Error: No PIN set.";

            string hexData = match.Groups[1].Value;

            byte[] salt = new byte[8];
            byte[] targetHash = new byte[32];
            
            ScryptEncoder encoder = new ScryptEncoder();

            return await Task.Run(() =>
            {
                // Parallel loop is much faster than Python
                string? foundPin = null;
                Parallel.For(0, 10000, (i, state) =>
                {
                    string pin = i.ToString("D4");            
                    if (i % 1000 == 0) progress.Report($"Checking {i}...");
                });

                return foundPin ?? "PIN not found.";
            });
        }
    }
}