using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Services.Steam;

namespace MKTL.WPF.ViewModels
{
    public partial class ManifestViewModel : ObservableObject
    {
        [ObservableProperty] private string _appId;
        [ObservableProperty] private string _steamPath = "C:\\Program Files (x86)\\Steam";
        [ObservableProperty] private string _log;

        private readonly ManifestService _service;

        public ManifestViewModel()
        {
            _service = new ManifestService();
            Log = "Ready.";
        }

        [RelayCommand]
        public async Task AddApp()
        {
            if (string.IsNullOrEmpty(AppId)) return;
            
            Log = $"Processing {AppId}...";
            bool success = await _service.InstallManifestAsync(AppId, SteamPath);
            
            Log = success ? "Installed successfully." : "Failed.";
        }
    }
}