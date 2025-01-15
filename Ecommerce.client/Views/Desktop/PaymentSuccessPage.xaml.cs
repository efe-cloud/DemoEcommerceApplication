using Ecommerce.client.ViewModels;
using Microsoft.Maui.Controls;

namespace Ecommerce.client.Views.Desktop
{
    public partial class PaymentSuccessPage : ContentPage
    {
        public PaymentSuccessPage(PaymentSuccessViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
