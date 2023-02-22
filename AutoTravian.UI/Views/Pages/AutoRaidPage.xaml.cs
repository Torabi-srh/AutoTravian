using Wpf.Ui.Common.Interfaces;

namespace AutoTravian.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AutoRaidPage : INavigableView<ViewModels.AutoRaidViewModel>
    {
        public ViewModels.AutoRaidViewModel ViewModel
        {
            get;
        }

        public AutoRaidPage(ViewModels.AutoRaidViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
