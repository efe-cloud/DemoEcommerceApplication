using Ecommerce.client.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Serilog;
using Serilog.Core;

namespace Ecommerce.client.Services
{
    public class CartApiService : ICartApiService
    {
        private readonly HttpClient _httpClient;

        public CartApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CartItem>> GetMyCartAsync()
        {
            var response = await _httpClient.GetAsync("api/Cart");

            // If the response is not successful, return an empty list
            if (!response.IsSuccessStatusCode)
                return new List<CartItem>();

            // Check if the response has content
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
                return new List<CartItem>();

            try
            {
                // Attempt to deserialize the JSON content
                var cart = JsonSerializer.Deserialize<ServerCart>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // If deserialization fails or Items is null, return an empty list
                if (cart == null || cart.Items == null)
                    return new List<CartItem>();

                // Map ServerCart to CartItem
                return cart.Items.Select(ci => new CartItem
                {
                    ProductId = ci.Product.Id,
                    Name = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    Image = ci.Product.Image
                }).ToList();
            }
            catch (JsonException ex)
            {
                // Log the exception for debugging purposes
                
                return new List<CartItem>();
            }
        }

        public async Task AddToCartAsync(int productId, int quantity)
        {
            // example: POST /api/Cart/add?productId=xx&quantity=yy
            var response = await _httpClient.PostAsync(
                $"api/Cart/add?productId={productId}&quantity={quantity}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveItemAsync(int productId)
        {
            var response = await _httpClient.DeleteAsync($"api/Cart/remove?productId={productId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearCartAsync()
        {
            var response = await _httpClient.DeleteAsync("api/Cart/clear");
            response.EnsureSuccessStatusCode();
        }
    }

    // Helper model for deserializing server's Cart
    public class ServerCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<ServerCartItem> Items { get; set; }
    }

    public class ServerCartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ServerProduct Product { get; set; }
    }

    public class ServerProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
}
