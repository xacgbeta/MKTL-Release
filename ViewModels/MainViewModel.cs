using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Services.Core;

namespace MKTL.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ConfigService _configService;

        [ObservableProperty]
        private object _currentViewModel;

        [ObservableProperty]
        private string _currentTitle = "Activation";

        public MainViewModel()
        {
            _configService = new ConfigService();
            CurrentViewModel = new ActivationViewModel(_configService, this);
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try { await _configService.LoadAsync(); }
            catch { /* Handle connection error */ }
        }

        public void NavigateToMain()
        {
            CurrentViewModel = new ManifestViewModel();
            CurrentTitle = "Manifest Tool";
        }

        [RelayCommand]
        public void Navigate(string dest)
        {
            switch (dest)
            {
                case "Manifest":
                    CurrentViewModel = new ManifestViewModel();
                    CurrentTitle = "Manifest Tool";
                    break;
                case "Denuvo":
                    CurrentViewModel = new DenuvoViewModel();
                    CurrentTitle = "Steam Ticket";
                    break;
                case "EA":
                    CurrentViewModel = new EaViewModel();
                    CurrentTitle = "EA Token";
                    break;
                case "Pin":
                    CurrentViewModel = new PinViewModel();
                    CurrentTitle = "Pin Recovery";
                    break;
                case "Updates":
                    CurrentViewModel = new UpdateControlViewModel();
                    CurrentTitle = "Update Manager";
                    break;
            }
        }
    }
}