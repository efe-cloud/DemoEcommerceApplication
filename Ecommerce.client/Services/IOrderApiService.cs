using Ecommerce.client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.client.Services
{
    public interface IOrderApiService
    {
        Task<int> CreateOrderAsync(PaymentDetails paymentDetails);
        Task<List<UserOrder>> GetMyOrdersAsync();
        Task<UserOrder> GetOrderByIdAsync(int orderId);
        Task<bool> ConfirmPaymentAsync(int orderId);
    }
}
