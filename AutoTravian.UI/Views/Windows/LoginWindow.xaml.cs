using AutoTravian.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace AutoTravian.UI.Views.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : INavigationWindow
    {

        public delegate void LoginEventHandler(bool sender, EventArgs e);
        public event LoginEventHandler OnLoggedIn;

        public LoginWindow(Window owner)
        {
            this.Owner = owner;
            InitializeComponent();
        }


        #region INavigationWindow methods

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        public Frame GetFrame()
        {
            throw new NotImplementedException();
        }

        public INavigation GetNavigation()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type pageType)
        {
            throw new NotImplementedException();
        }

        public void SetPageService(IPageService pageService)
        {
            throw new NotImplementedException();
        }

        #endregion INavigationWindow methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnLoggedIn(true, EventArgs.Empty);
        }
    }
}
