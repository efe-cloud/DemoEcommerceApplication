using Ecommerce.client.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Ecommerce.client.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse> AddProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Products", product);
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("api/Products/categories");
            return await response.Content.ReadFromJsonAsync<List<Category>>();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync($"api/Products/categories/{categoryId}");
            return await response.Content.ReadFromJsonAsync<List<Product>>();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("api/Products");
            return await response.Content.ReadFromJsonAsync<List<Product>>();
        }
    }
}
