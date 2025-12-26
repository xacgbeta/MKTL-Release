using System.Diagnostics;
using System.Management; // Add reference to System.Management
using System.Net.Http;
using Microsoft.Win32;

namespace MKTL_WPF.Helpers
{
    public static class SystemHelper
    {
        public static string GetHWID()
        {
            try
            {
                // Try getting UUID via WMI
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct");
                foreach (ManagementObject share in searcher.Get())
                {
                    return share["UUID"].ToString();
                }
            }
            catch { }

            // Fallback to Registry MachineGuid
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography");
                return key?.GetValue("MachineGuid")?.ToString() ?? "Error-HWID";
            }
            catch { return "Error-HWID"; }
        }

    }
}