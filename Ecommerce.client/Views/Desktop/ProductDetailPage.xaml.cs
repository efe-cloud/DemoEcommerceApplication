using Ecommerce.client.ViewModels;

namespace Ecommerce.client.Views.Desktop
{
    public partial class ProductDetailPage : ContentPage
    {
        public ProductDetailPage(ProductDetailPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
