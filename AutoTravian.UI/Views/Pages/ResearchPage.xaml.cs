using Wpf.Ui.Common.Interfaces;

namespace AutoTravian.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ResearchPage : INavigableView<ViewModels.ResearchViewModel>
    {
        public ViewModels.ResearchViewModel ViewModel
        {
            get;
        }

        public ResearchPage(ViewModels.ResearchViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
