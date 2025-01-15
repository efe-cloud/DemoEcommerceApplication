using ECommerce.Api.Data;
using Ecommerce.library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Services
{
    public class CartDbService : ICartDbService
    {
        private readonly AppDbScope _db;

        public CartDbService(AppDbScope db)
        {
            _db = db;
        }

        public async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }
            return cart;
        }

        public async Task AddProductToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await GetOrCreateCartAsync(userId);

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.Items.Add(newItem);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task RemoveItemAsync(int userId, int productId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null) return;

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null) return;

            _db.CartItems.RemoveRange(cart.Items);
            await _db.SaveChangesAsync();
        }
    }
}
