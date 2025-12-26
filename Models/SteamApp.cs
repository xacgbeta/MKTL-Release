namespace MKTL.WPF.Models
{
    public class SteamApp
    {
        public string AppId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string InstallPath { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }

        public string DisplayName => $"{Name} ({AppId})";
    }
}