using Ecommerce.library.Responses;
using Ecommerce.library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Services
{
    public interface IProductService
    {
        Task<ServiceResponse> AddProductAsync(Product product);
        Task<ServiceResponse> UpdateProductAsync(Product product);
        Task<ServiceResponse> DeleteProductAsync(int id);
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId); 
    }
}
