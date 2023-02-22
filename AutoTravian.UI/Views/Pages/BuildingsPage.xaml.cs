using Wpf.Ui.Common.Interfaces;

namespace AutoTravian.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class BuildingsPage : INavigableView<ViewModels.BuildingsViewModel>
    {
        public ViewModels.BuildingsViewModel ViewModel
        {
            get;
        }

        public BuildingsPage(ViewModels.BuildingsViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
