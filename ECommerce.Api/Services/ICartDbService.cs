using Ecommerce.library.Models;
using System.Threading.Tasks;

namespace ECommerce.Api.Services
{
    public interface ICartDbService
    {
        Task<Cart> GetOrCreateCartAsync(int userId);
        Task AddProductToCartAsync(int userId, int productId, int quantity);
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task RemoveItemAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
