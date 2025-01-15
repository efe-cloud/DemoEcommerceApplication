using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Ecommerce.client.Messages;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ecommerce.client.Services
{
    public class OrderDataService : ObservableObject
    {
        private readonly IOrderApiService _orderService;

        public OrderDataService(IOrderApiService orderService)
        {
            _orderService = orderService;
            Orders = new ObservableCollection<UserOrder>();
        }

        public ObservableCollection<UserOrder> Orders { get; }

        public async Task LoadOrdersAsync()
        {
            var orders = await _orderService.GetMyOrdersAsync();
            Orders.Clear();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
        }

        public async Task AddOrderAsync(int orderId)
        {
            
            var newOrder = await _orderService.GetOrderByIdAsync(orderId); 
            if (newOrder != null)
            {
                Orders.Add(newOrder);
            }
        }
    }
}
