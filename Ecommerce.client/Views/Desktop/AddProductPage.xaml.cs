using Ecommerce.client.ViewModels;

namespace Ecommerce.client.Views.Desktop;

public partial class AddProductPage : ContentPage
{
    public AddProductPage(AddProductPageViewModel addProductPageViewModel)
	{
		InitializeComponent();
		BindingContext = addProductPageViewModel;
    }


}