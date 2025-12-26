using System.Runtime.InteropServices;
using System.Text;

namespace MKTL_WPF.Services
{
    public class SteamTicketService
    {
        [DllImport("steam_api64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool SteamAPI_InitFlat(IntPtr err);

        [DllImport("steam_api64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SteamAPI_SteamUser_v023();
        
        public string GenerateTicket(uint appId)
        {
            Environment.SetEnvironmentVariable("SteamAppId", appId.ToString());
            
            if (!SteamAPI_InitFlat(IntPtr.Zero))
                throw new Exception("Steam API Init failed");

            return "Ticket_Base64_String";
        }
    }
}