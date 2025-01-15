
using Ecommerce.client.ViewModels;

namespace Ecommerce.client.Views.Desktop
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
