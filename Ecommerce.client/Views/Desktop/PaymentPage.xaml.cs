using Ecommerce.client.ViewModels;
using Microsoft.Maui.Controls;

namespace Ecommerce.client.Views.Desktop
{
    public partial class PaymentPage : ContentPage
    {
        public PaymentPage(PaymentViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PaymentViewModel vm)
            {
                await vm.InitializeAsync();
            }
        }
    }
}
