using AutoTravian.UI.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace AutoTravian.UI.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {
        public ViewModels.MainWindowViewModel ViewModel
        {
            get;
        }

        public MainWindow(ViewModels.MainWindowViewModel viewModel, IPageService pageService, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);
        }

        #region INavigationWindow methods

        public Frame GetFrame()
            => RootFrame;

        public INavigation GetNavigation()
            => RootNavigation;

        public bool Navigate(Type pageType)
            => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService)
            => RootNavigation.PageService = pageService;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }
        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            using (UserDbContext context = new())
            {
                App.CurrentUser = await context.UserTable.FirstOrDefaultAsync() ?? new Models.Entities.UserEntity();
                if (string.IsNullOrEmpty(App.CurrentUser.User) || string.IsNullOrEmpty(App.CurrentUser.Password) || !App.CurrentUser.Remember)
                {
                    Blurrry.Visibility = Visibility.Visible;
                    LoginWindow lw = new LoginWindow(this);
                    lw.OnLoggedIn += (sender, e) =>
                    {
                        lw.Close();
                        Blurrry.Visibility = Visibility.Hidden;
                    };
                    lw.ShowDialog();
                }
            }
        }
    }
}