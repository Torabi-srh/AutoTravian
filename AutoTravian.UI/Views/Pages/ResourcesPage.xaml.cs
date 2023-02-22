using Wpf.Ui.Common.Interfaces;

namespace AutoTravian.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ResourcesPage : INavigableView<ViewModels.ResourcesViewModel>
    {
        public ViewModels.ResourcesViewModel ViewModel
        {
            get;
        }

        public ResourcesPage(ViewModels.ResourcesViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
