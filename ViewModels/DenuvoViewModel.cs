using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Services.Denuvo;
using MKTL.WPF.Services.Steam;
using System.Collections.ObjectModel;
using System.Windows.Forms; // For FolderBrowserDialog

namespace MKTL.WPF.ViewModels
{
    public partial class DenuvoViewModel : ObservableObject
    {
        [ObservableProperty] private string _appId;
        [ObservableProperty] private string _outputDir;
        [ObservableProperty] private string _log;
        [ObservableProperty] private ObservableCollection<AccountInfo> _accounts;
        [ObservableProperty] private AccountInfo _selectedAccount;

        private readonly AccountService _accountService;

        public DenuvoViewModel()
        {
            _accountService = new AccountService();
            Accounts = new ObservableCollection<AccountInfo>(_accountService.GetAccounts());
            Log = "Ready to generate tickets.";
        }

        [RelayCommand]
        public void Browse()
        {
            using var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                OutputDir = dialog.SelectedPath;
        }

        [RelayCommand]
        public async Task Generate()
        {
            if (string.IsNullOrEmpty(AppId) || string.IsNullOrEmpty(OutputDir)) 
            {
                Log += "\nMissing fields.";
                return;
            }

            Log += $"\nGenerating ticket for {AppId}...";

            await Task.Run(() =>
            {
                if (!uint.TryParse(AppId, out uint id)) return;

                var result = TicketInterop.Generate(id);
                if (result.Ticket != null)
                {
                    // Logic to create zip file structure (Goldberg/Coldloader)
                    // ... (File.WriteAllText config.ini etc)
                    Log += "\nSuccess! Ticket generated.";
                }
                else
                {
                    Log += "\nFailed. Ensure Steam is running and you own the game.";
                }
            });
        }
    }
}