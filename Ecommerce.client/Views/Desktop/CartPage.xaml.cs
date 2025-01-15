using Ecommerce.client.ViewModels;

namespace Ecommerce.client.Views.Desktop
{
    public partial class CartPage : ContentPage
    {
        private readonly CartViewModel _viewModel;

        public CartPage(CartViewModel vm)
        {
            InitializeComponent();
            BindingContext = _viewModel = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Force the cart to refresh each time the page is displayed
            await _viewModel.RefreshAsync();
        }
    }
}
