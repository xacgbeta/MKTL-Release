using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKTL.WPF.Helpers;

namespace MKTL.WPF.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty] private string _appVersion = "3.0.0";
        [ObservableProperty] private string _credits;

        public SettingsViewModel()
        {
            Credits = "Developed by Helstorm [Mike]\n\n" +
                      "Credits to: Daniel Roster, Morrenus, Oureveryday, NotAndreh, Detanup01, Anadius.\n\n" +
                      "This software is for educational purposes only.";
        }

        [RelayCommand]
        public void JoinDiscord()
        {
            ProcessHelper.OpenUrl("https://discord.gg/your-link-here");
        }

        [RelayCommand]
        public void CheckUpdates()
        {
            // Trigger UpdateService logic here
        }
    }
}