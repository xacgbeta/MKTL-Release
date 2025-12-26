using System.Management; // Requires System.Management NuGet
using Microsoft.Win32;

namespace MKTL.WPF.Helpers
{
    public static class HardwareHelper
    {
        public static string GetHWID()
        {
            try
            {
                var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_Processor");
                foreach (ManagementObject mo in mbs.Get()) return mo["ProcessorId"].ToString();
            }
            catch { }
            
            // Fallback
            return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography", "MachineGuid", "Error")?.ToString() ?? "Error";
        }
    }
}