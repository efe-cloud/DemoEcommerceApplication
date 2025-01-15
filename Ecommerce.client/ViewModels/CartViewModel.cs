using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Ecommerce.client.ViewModels
{
    public partial class CartViewModel : ObservableObject
    {
        private readonly ICartApiService _cartService;
        private readonly IOrderApiService _orderService;

        public CartViewModel(ICartApiService cartService, IOrderApiService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
            CartItems = new ObservableCollection<CartItem>();
        }

        public ObservableCollection<CartItem> CartItems { get; }

        [ObservableProperty]
        private bool isLoggedIn;

        [ObservableProperty]
        private string title = "Your Cart";

        [ObservableProperty]
        private double totalPrice;

        // Inverse property to replace InverseBoolConverter
        public bool IsNotLoggedIn => !IsLoggedIn;

        partial void OnIsLoggedInChanged(bool value)
        {
            OnPropertyChanged(nameof(IsNotLoggedIn));
        }

        [RelayCommand]
        public async Task RefreshAsync()
        {
            
            IsLoggedIn = await CheckLoginStatusAsync();

            if (IsLoggedIn)
            {
                var items = await _cartService.GetMyCartAsync();
                CartItems.Clear();
                foreach (var item in items)
                {
                    CartItems.Add(item);
                }
                TotalPrice = CalculateTotalPrice();
            }
            else
            {
                CartItems.Clear();
                TotalPrice = 0;
            }
        }

        private double CalculateTotalPrice()
        {
            double total = 0;
            foreach (var item in CartItems)
            {
                total += item.TotalPrice;
            }
            return total;
        }

        private async Task<bool> CheckLoginStatusAsync()
        {
            
            return await Task.FromResult(true); 
        }

        [RelayCommand]
        public async Task RemoveItemAsync(CartItem item)
        {
            await _cartService.RemoveItemAsync(item.ProductId);
            CartItems.Remove(item);
            TotalPrice = CalculateTotalPrice();
        }

        [RelayCommand]
        public async Task ClearCartAsync()
        {
            await _cartService.ClearCartAsync();
            CartItems.Clear();
            TotalPrice = 0;
        }

        [RelayCommand]
        public async Task GoToPaymentPageAsync()
        {
            // Navigate to PaymentPage using Shell navigation
            await Shell.Current.GoToAsync(nameof(Views.Desktop.PaymentPage));
        }
    }
}
