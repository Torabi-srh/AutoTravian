using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace AutoTravian.UI.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "Auto Travian Manager";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage),
                },
                new NavigationItem()
                {
                    Content = "Resources",
                    PageTag = "resources",
                    Icon = SymbolRegular.FoodGrains24,
                    PageType = typeof(Views.Pages.ResourcesPage),
                },
                new NavigationItem()
                {
                    Content = "Tasks",
                    PageTag = "tasks",
                    Icon = SymbolRegular.TaskListSquareSettings20,
                    PageType = typeof(Views.Pages.TaskManagerPage),
                },
                new NavigationItem()
                {
                    Content = "Buildings",
                    PageTag = "buildings",
                    Icon = SymbolRegular.BuildingGovernment32,
                    PageType = typeof(Views.Pages.TaskManagerPage),
                },
                new NavigationItem()
                {
                    Content = "Raid",
                    PageTag = "raid",
                    Icon = SymbolRegular.TargetArrow24,
                    PageType = typeof(Views.Pages.AutoRaidPage),
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage),
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
        }
    }
}
