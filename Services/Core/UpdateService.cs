using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace MKTL.WPF.Services.Core
{
    public class UpdateService
    {
        public async Task<bool> CheckAndInstallUpdate(string currentVersion)
        {
            // 1. Check API
            using var client = new HttpClient();
            // Mock URL based on original script config
            string url = $""; 
            
            // Assume we got a JSON response with { "update_available": true, "url": "..." }
            bool updateAvailable = false; 
            string downloadUrl = ""; 

            if (!updateAvailable) return false;

            // 2. Download
            string tempPath = Path.Combine(Path.GetTempPath(), "MKTL_New.exe");
            var bytes = await client.GetByteArrayAsync(downloadUrl);
            await File.WriteAllBytesAsync(tempPath, bytes);

            // 3. Create Batch Script
            string currentExe = Process.GetCurrentProcess().MainModule.FileName;
            string batchPath = Path.Combine(Path.GetTempPath(), "mktl_updater.bat");

            string script = $@"
@echo off
timeout /t 2 /nobreak > NUL
del ""{currentExe}""
move ""{tempPath}"" ""{currentExe}""
start """" ""{currentExe}""
del ""%~f0""
";
            await File.WriteAllTextAsync(batchPath, script);

            // 4. Run Script and Exit
            Process.Start(new ProcessStartInfo
            {
                FileName = batchPath,
                CreateNoWindow = true,
                UseShellExecute = true
            });

            Environment.Exit(0);
            return true;
        }
    }
}