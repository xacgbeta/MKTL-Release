using System.Diagnostics;
using System.IO;

namespace MKTL.WPF.Services.Tools
{
    public class SteamlessService
    {
        private const string CliPath = "Assets/Binaries/Steamless.CLI.exe";

        public async Task<string> UnpackFolderAsync(string folderPath)
        {
            if (!File.Exists(CliPath)) return "Error: Steamless CLI not found.";

            var exes = Directory.GetFiles(folderPath, "*.exe", SearchOption.AllDirectories);
            int successCount = 0;

            await Task.Run(() => 
            {
                foreach (var file in exes)
                {
                    if (file.Contains(".unpacked") || file.Contains(".bak")) continue;

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = CliPath,
                        Arguments = $"\"{file}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using var proc = Process.Start(startInfo);
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();

                    if (output.Contains("Unpacked file written"))
                    {
                        successCount++;
                    }
                }
            });

            return $"Processed {exes.Length} files. {successCount} unpacked successfully.";
        }
    }
}