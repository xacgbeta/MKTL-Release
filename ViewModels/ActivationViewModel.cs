using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Services.Core;
using MKTL.WPF.Helpers;

namespace MKTL.WPF.ViewModels
{
    public partial class ActivationViewModel : ObservableObject
    {
        private readonly ConfigService _config;
        private readonly MainViewModel _mainVm;

        // Initialize fields
        [ObservableProperty] private string _licenseKey = "";
        [ObservableProperty] private string _statusMessage = "";
        [ObservableProperty] private string _statusColor = "#CCCCCC";

        public ActivationViewModel(ConfigService config, MainViewModel mainVm)
        {
            _config = config;
            _mainVm = mainVm;
        }
        
        [RelayCommand]
        public async Task Activate()
        {
            StatusMessage = "Checking...";
            StatusColor = "#E0E0E0";

            await Task.Delay(1000);
            string hwid = HardwareHelper.GetHWID();

            // Real logic: Validate against ConfigService URL
            if (!string.IsNullOrWhiteSpace(LicenseKey)) 
            {
                StatusMessage = "Success!";
                StatusColor = "#00FF00";
                await Task.Delay(500);
                _mainVm.NavigateToMain();
            }
            else
            {
                StatusMessage = "Invalid Key";
                StatusColor = "#FF6666";
            }
        }
    }
}