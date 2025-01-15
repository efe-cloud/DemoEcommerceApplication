using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Ecommerce.client.Messages;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ecommerce.client.ViewModels
{
    public partial class OrderHistoryViewModel : ObservableObject, IRecipient<NewOrderCreatedMessage>
    {
        private readonly OrderDataService _orderDataService;

        public OrderHistoryViewModel(OrderDataService orderDataService)
        {
            _orderDataService = orderDataService;
            Orders = new ObservableCollection<UserOrder>();

            // Register to receive NewOrderCreatedMessage
            WeakReferenceMessenger.Default.Register<NewOrderCreatedMessage>(this);
        }

        [ObservableProperty]
        private bool isLoading;

        public ObservableCollection<UserOrder> Orders { get; }

        [ObservableProperty]
        private string statusMessage;

        // Implement the IRecipient interface
        public async void Receive(NewOrderCreatedMessage message)
        {
            // Refresh the orders
            await LoadOrdersAsync();
        }

        [RelayCommand]
        public async Task LoadOrdersAsync()
        {
            IsLoading = true;
            StatusMessage = string.Empty;

            try
            {
                await _orderDataService.LoadOrdersAsync();

                Orders.Clear();
                foreach (var order in _orderDataService.Orders)
                {
                    order.TotalAmount = order.Items.Sum(item => item.PriceAtCheckout * item.Quantity);
                    Orders.Add(order);
                }

                if (Orders.Count == 0)
                {
                    StatusMessage = "You have no orders.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to load orders: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
