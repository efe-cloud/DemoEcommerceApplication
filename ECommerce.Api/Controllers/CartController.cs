using ECommerce.Api.Services;
using Ecommerce.library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartDbService _cartDbService;

        public CartController(ICartDbService cartDbService)
        {
            _cartDbService = cartDbService;
        }

        // Helpers
        private int GetUserIdFromToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userIdString);
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetMyCart()
        {
            var userId = GetUserIdFromToken();
            var cart = await _cartDbService.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = GetUserIdFromToken();
            await _cartDbService.AddProductToCartAsync(userId, productId, quantity);
            return Ok("Product added to cart");
        }

        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveItem(int productId)
        {
            var userId = GetUserIdFromToken();
            await _cartDbService.RemoveItemAsync(userId, productId);
            return Ok("Item removed from cart");
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearCart()
        {
            var userId = GetUserIdFromToken();
            await _cartDbService.ClearCartAsync(userId);
            return Ok("Cart cleared");
        }
    }
}
