using AutoTravian.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;

namespace AutoTravian.UI.ViewModels
{
    public partial class AutoRaidViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

       // [ObservableProperty]
        

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            
        }
    }
}
