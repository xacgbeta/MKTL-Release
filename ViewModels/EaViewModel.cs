using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Services.Denuvo;
using System.Threading.Tasks;

namespace MKTL.WPF.ViewModels
{
    public partial class EaViewModel : ObservableObject
    {
        private readonly EaMemoryService _eaService;

        [ObservableProperty] private string _tokenOutput = "";
        [ObservableProperty] private string _log = "";
        [ObservableProperty] private bool _isBusy;

        public EaViewModel()
        {
            _eaService = new EaMemoryService();
            Log = "Ready to scan EA Desktop memory.";
        }

        [RelayCommand]
        public async Task GenerateToken()
        {
            IsBusy = true;
            Log = "Scanning memory for 'authorization=Bearer' token...";
            TokenOutput = "";

            string? token = await _eaService.GetEaTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                TokenOutput = token;
                Log = "Success! Token found.";
                System.Windows.Clipboard.SetText(token); 
            }
            else
            {
                Log = "Failed. Ensure EA Desktop is running and you are logged in.";
            }

            IsBusy = false;
        }
    }
}