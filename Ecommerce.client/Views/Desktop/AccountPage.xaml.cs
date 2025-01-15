using Ecommerce.client.ViewModels;

namespace Ecommerce.client.Views.Desktop
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage(AccountViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
