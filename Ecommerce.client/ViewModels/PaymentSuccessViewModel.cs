using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Ecommerce.client.ViewModels
{
    public partial class PaymentSuccessViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isLoading; // ✅ Define 'IsLoading' correctly

        public PaymentSuccessViewModel()
        {
            ShowSuccessAndNavigate();
        }

        private async void ShowSuccessAndNavigate()
        {
            IsLoading = true;  //  Show loading indicator
            await Task.Delay(3000); // ✅ Wait 3 seconds
            IsLoading = false; //  Hide loading indicator

            //  Use relative navigation instead of absolute
            await Shell.Current.GoToAsync("OrderHistoryPage");
        }
    }
}
