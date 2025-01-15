using Ecommerce.client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.client.Services
{
    public interface ICartApiService
    {
        Task<List<CartItem>> GetMyCartAsync();
        Task AddToCartAsync(int productId, int quantity);
        Task RemoveItemAsync(int productId);
        Task ClearCartAsync();
    }
}
