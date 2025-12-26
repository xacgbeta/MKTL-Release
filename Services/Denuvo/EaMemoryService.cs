using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MKTL.WPF.Services.Denuvo
{
    public class EaMemoryService
    {
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO { public ushort wProcessorArchitecture; public ushort wReserved; public uint dwPageSize; public IntPtr lpMinimumApplicationAddress; public IntPtr lpMaximumApplicationAddress; public IntPtr dwActiveProcessorMask; public uint dwNumberOfProcessors; public uint dwProcessorType; public uint dwAllocationGranularity; public ushort wProcessorLevel; public ushort wProcessorRevision; }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION { public IntPtr BaseAddress; public IntPtr AllocationBase; public uint AllocationProtect; public IntPtr RegionSize; public uint State; public uint Protect; public uint Type; }

        public async Task<string?> GetEaTokenAsync()
        {
            return await Task.Run(() =>
            {
                var procs = Process.GetProcessesByName("EADesktop");
                if (procs.Length == 0) return null;

                IntPtr hProc = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, procs[0].Id);
                if (hProc == IntPtr.Zero) return null;

                try
                {
                    GetSystemInfo(out SYSTEM_INFO sysInfo);
                    IntPtr minAddr = sysInfo.lpMinimumApplicationAddress;
                    IntPtr maxAddr = sysInfo.lpMaximumApplicationAddress;
                    long procMin = (long)minAddr;
                    long procMax = (long)maxAddr;

                    Regex pattern = new Regex(@"authorization=Bearer ([a-zA-Z0-9\._\-]+)", RegexOptions.Compiled);

                    IntPtr currentAddr = minAddr;
                    MEMORY_BASIC_INFORMATION memInfo = new MEMORY_BASIC_INFORMATION();

                    while (procMin < procMax)
                    {
                        VirtualQueryEx(hProc, currentAddr, out memInfo, (uint)Marshal.SizeOf(memInfo));

                        if (memInfo.State == 0x1000 && (memInfo.Protect == 0x04 || memInfo.Protect == 0x02))
                        {
                            byte[] buffer = new byte[(int)memInfo.RegionSize];
                            if (ReadProcessMemory(hProc, memInfo.BaseAddress, buffer, buffer.Length, out int bytesRead))
                            {
                                // Convert valid ASCII bytes to string to scan
                                string chunk = Encoding.ASCII.GetString(buffer);
                                var match = pattern.Match(chunk);
                                if (match.Success)
                                {
                                    return match.Groups[1].Value;
                                }
                            }
                        }

                        procMin += (long)memInfo.RegionSize;
                        currentAddr = new IntPtr(procMin);
                    }
                }
                finally
                {
                    CloseHandle(hProc);
                }
                return null;
            });
        }
    }
}