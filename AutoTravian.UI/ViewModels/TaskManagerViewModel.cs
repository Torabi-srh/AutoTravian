using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace AutoTravian.UI.ViewModels
{

    public class TaskManagerViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;


        private ICommand _navigateCommand;

        private ICommand _openWindowCommand;

        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand<string>(OnNavigate);

        public ICommand OpenWindowCommand => _openWindowCommand ??= new RelayCommand<string>(OnOpenWindow);

        public TaskManagerViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedTo()
        {
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "Wpf.Ui.Demo");
        }

        public void OnNavigatedFrom()
        {
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "Wpf.Ui.Demo");
        }

        private void OnNavigate(string parameter)
        {
            switch (parameter)
            {
               
            }
        }

        private void OnOpenWindow(string parameter)
        {
        }
    }
}
