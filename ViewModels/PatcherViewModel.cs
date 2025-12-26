using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Models;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace MKTL.WPF.ViewModels
{
    public partial class PatcherViewModel : ObservableObject
    {
        [ObservableProperty] private string _searchTerm;
        [ObservableProperty] private string _selectedFolder;
        [ObservableProperty] private string _log = "Select a game and target folder.";
        [ObservableProperty] private bool _isBusy;
        [ObservableProperty] private double _progress;

        [RelayCommand]
        public void Browse()
        {
            using var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectedFolder = dialog.SelectedPath;
            }
        }

        [RelayCommand]
        public async Task PatchGame()
        {
            if (string.IsNullOrEmpty(SearchTerm) || string.IsNullOrEmpty(SelectedFolder))
            {
                Log = "Error: Please enter a game name and select a folder.";
                return;
            }

            IsBusy = true;
            Progress = 0;
            Log = $"Fetching data for {SearchTerm}...";

            try
            {
                // Simulate Download Logic
                // In real app: Fetch list from API -> Find Game -> Download Zip
                await Task.Run(async () =>
                {
                    for (int i = 0; i <= 100; i+=10)
                    {
                        await Task.Delay(200); // Simulate network
                        Progress = i;
                        Log = $"Downloading... {i}%";
                    }
                    
                    Log = "Extracting files...";
                    await Task.Delay(1000); // Simulate Unrar

                    Log = "Patch applied successfully!";
                });
            }
            catch (Exception ex)
            {
                Log = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}