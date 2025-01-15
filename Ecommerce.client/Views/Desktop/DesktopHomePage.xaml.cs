using Ecommerce.client.ViewModels;

namespace Ecommerce.client.Views.Desktop
{
    public partial class DesktopHomePage : ContentPage
    {
        public DesktopHomePage(DesktopHomePageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
