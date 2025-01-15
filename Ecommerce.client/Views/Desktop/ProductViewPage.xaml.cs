using Ecommerce.client.ViewModels;
using Microsoft.Maui.Controls;

namespace Ecommerce.client.Views.Desktop
{
    public partial class ProductsViewPage : ContentPage
    {
        public ProductsViewPage(ProductsViewViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
