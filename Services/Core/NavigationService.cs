using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MKTL.WPF.Services.Core
{
    public partial class NavigationService : ObservableObject
    {
        [ObservableProperty]
        private ObservableObject _currentView;

        public void NavigateTo<T>() where T : ObservableObject
        {
            // Logic to resolve ViewModel from DI container (explained in App.xaml)
        }
    }
}