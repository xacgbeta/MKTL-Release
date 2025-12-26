using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Services.Steam;
using System.Collections.ObjectModel;
using System.IO;

namespace MKTL.WPF.ViewModels
{
    public class GameUpdateItem
    {
        public string AppId { get; set; }
        public string Name { get; set; } // Would fetch from API in real app
        public bool IsEnabled { get; set; }
        public string StatusColor => IsEnabled ? "#00FF00" : "#FF6666";
    }

    public partial class UpdateControlViewModel : ObservableObject
    {
        private readonly UpdateTogglerService _toggler;
        private readonly AccountService _accService; // To find steam path

        [ObservableProperty] private ObservableCollection<GameUpdateItem> _games;
        [ObservableProperty] private GameUpdateItem _selectedGame;

        public UpdateControlViewModel()
        {
            _toggler = new UpdateTogglerService();
            _accService = new AccountService(); // Helper to find path
            Games = new ObservableCollection<GameUpdateItem>();
            RefreshList();
        }

        [RelayCommand]
        public void RefreshList()
        {
            Games.Clear();
            // Logic to find path similar to AccountService
            string steamPath = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", "")?.ToString()?.Replace("/", "\\");
            if (steamPath == null) return;

            string pluginDir = Path.Combine(steamPath, "config", "stplug-in");
            if (!Directory.Exists(pluginDir)) return;

            foreach (var file in Directory.GetFiles(pluginDir, "*.lua"))
            {
                string content = File.ReadAllText(file);
                string appId = Path.GetFileNameWithoutExtension(file);
                
                // Check if commented out
                bool isEnabled = !content.TrimStart().StartsWith("--");
                
                Games.Add(new GameUpdateItem 
                { 
                    AppId = appId, 
                    Name = $"Game {appId}", // In real app, fetch name from Web API
                    IsEnabled = isEnabled 
                });
            }
        }

        [RelayCommand]
        public void ToggleSelected()
        {
            if (SelectedGame == null) return;

            // Logic to find path
            string steamPath = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", "")?.ToString()?.Replace("/", "\\");
            
            // Toggle state
            bool newState = !SelectedGame.IsEnabled;
            _toggler.ToggleUpdate(steamPath, SelectedGame.AppId, newState);
            
            // Update UI
            SelectedGame.IsEnabled = newState;
            // Force UI refresh (simple way is to reload list or implement INotifyPropertyChanged on Item)
            RefreshList(); 
        }
    }
}