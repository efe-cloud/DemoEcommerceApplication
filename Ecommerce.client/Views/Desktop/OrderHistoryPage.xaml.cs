using Ecommerce.client.ViewModels;
using Microsoft.Maui.Controls;

namespace Ecommerce.client.Views.Desktop
{
    public partial class OrderHistoryPage : ContentPage
    {
        public OrderHistoryPage(OrderHistoryViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is OrderHistoryViewModel vm)
            {
                await vm.LoadOrdersAsync();
            }
        }
    }
}
