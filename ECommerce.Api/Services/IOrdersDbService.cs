using Ecommerce.library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Services
{
    public interface IOrdersDbService
    {
        Task<int> CreateOrderAsync(int userId);
        Task<List<Order>> GetOrdersByUserAsync(int userId);
        Task<bool> ConfirmPaymentAsync(int orderId);
        
    }
}
