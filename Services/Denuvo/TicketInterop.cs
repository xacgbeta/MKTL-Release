using System.Runtime.InteropServices;
using System.Text;

namespace MKTL.WPF.Services.Denuvo
{
    public static class TicketInterop
    {
        const string DllName = "steam_api64.dll"; 

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SteamAPI_InitFlat(out IntPtr err);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SteamAPI_Shutdown();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SteamAPI_SteamUser_v023();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SteamAPI_ISteamUser_RequestEncryptedAppTicket(IntPtr instance, IntPtr data, int dataSize);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SteamAPI_ISteamUser_GetEncryptedAppTicket(IntPtr instance, byte[] buffer, int maxBuffer, out uint realSize);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong SteamAPI_ISteamUser_GetSteamID(IntPtr instance);

        public static (string? Ticket, string? SteamId) Generate(uint appId)
        {
            // Set Environment Variable so Steam knows which app we are "playing"
            Environment.SetEnvironmentVariable("SteamAppId", appId.ToString());
            Environment.SetEnvironmentVariable("SteamGameId", appId.ToString());

            if (!SteamAPI_InitFlat(out _)) return (null, null);

            try
            {
                IntPtr user = SteamAPI_SteamUser_v023();
                if (user == IntPtr.Zero) return (null, null);

                // Request Ticket
                SteamAPI_ISteamUser_RequestEncryptedAppTicket(user, IntPtr.Zero, 0);
                
                // Wait for Steam callback (Python slept, so we sleep)
                Thread.Sleep(2000); 

                byte[] buffer = new byte[2048];
                if (SteamAPI_ISteamUser_GetEncryptedAppTicket(user, buffer, 2048, out uint realSize))
                {
                    byte[] actualTicket = new byte[realSize];
                    Array.Copy(buffer, actualTicket, realSize);
                    
                    ulong steamId = SteamAPI_ISteamUser_GetSteamID(user);
                    return (Convert.ToBase64String(actualTicket), steamId.ToString());
                }
            }
            finally
            {
                SteamAPI_Shutdown();
            }
            return (null, null);
        }
    }
}