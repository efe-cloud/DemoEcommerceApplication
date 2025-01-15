
using Ecommerce.client.Models;

namespace Ecommerce.client.Services
{
    public interface IProductService
    {
        
        Task<List<Product>> GetProductsAsync();
        
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<ServiceResponse> AddProductAsync(Product product);
    }
}
