
using Ecommerce.client.ViewModels;

namespace Ecommerce.client.Views.Desktop
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
