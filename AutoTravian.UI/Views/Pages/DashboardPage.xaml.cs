using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;

namespace AutoTravian.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public DashboardPage(ViewModels.DashboardViewModel viewModel)
        {
            ViewModel = viewModel; 
            InitializeComponent();
        }
        protected override async void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

        }
        private void TaskbarStateComboBox_OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is not System.Windows.Controls.ComboBox comboBox)
                return;

            var parentWindow = System.Windows.Window.GetWindow(this);

            if (parentWindow == null)
                return;

            var selectedIndex = comboBox.SelectedIndex;

            switch (selectedIndex)
            {
                case 1:
                    Wpf.Ui.TaskBar.TaskBarProgress.SetValue(
                        parentWindow,
                        Wpf.Ui.TaskBar.TaskBarProgressState.Normal,
                        80);
                    break;

                case 2:
                    Wpf.Ui.TaskBar.TaskBarProgress.SetValue(
                        parentWindow,
                        Wpf.Ui.TaskBar.TaskBarProgressState.Error,
                        80);
                    break;

                case 3:
                    Wpf.Ui.TaskBar.TaskBarProgress.SetValue(
                        parentWindow,
                        Wpf.Ui.TaskBar.TaskBarProgressState.Paused,
                        80);
                    break;

                case 4:
                    Wpf.Ui.TaskBar.TaskBarProgress.SetValue(
                        parentWindow,
                        Wpf.Ui.TaskBar.TaskBarProgressState.Indeterminate,
                        80);
                    break;

                default:
                    Wpf.Ui.TaskBar.TaskBarProgress.SetState(parentWindow, Wpf.Ui.TaskBar.TaskBarProgressState.None);
                    break;
            }
        }
    }
}