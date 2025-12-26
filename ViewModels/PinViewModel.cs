using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Services.Steam;

namespace MKTL.WPF.ViewModels
{
    public partial class PinViewModel : ObservableObject
    {
        private readonly PinRecoveryService _pinService;

        [ObservableProperty] private string _resultText;
        [ObservableProperty] private string _statusText = "Ready";
        [ObservableProperty] private double _progressValue;
        [ObservableProperty] private bool _isBusy;

        public PinViewModel()
        {
            _pinService = new PinRecoveryService();
        }

        [RelayCommand]
        public async Task CrackPin()
        {
            IsBusy = true;
            ResultText = "";
            StatusText = "Initializing...";
            ProgressValue = 0;

            var progress = new Progress<string>(msg => 
            {
                StatusText = msg;
                if (int.TryParse(msg.Replace("Checking ", "").Replace("...", ""), out int current))
                {
                    ProgressValue = (current / 10000.0) * 100;
                }
            });

            string result = await _pinService.RecoverPinAsync(progress);
            
            ResultText = result;
            StatusText = result.Contains("Error") ? "Failed" : "Complete";
            IsBusy = false;
        }
    }
}