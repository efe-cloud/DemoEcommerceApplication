using ECommerce.Api.Data;
using Ecommerce.library.Models;
using Ecommerce.library.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbScope _appDbScope;

        public ProductService(AppDbScope appDbScope)
        {
            _appDbScope = appDbScope;
        }

        public async Task<ServiceResponse> AddProductAsync(Product product)
        {
            if (product == null)
                return new ServiceResponse() { Message = "Bad Request", Success = false };

            var existingProduct = await _appDbScope.Products
                .Where(p => p.Name.ToLower() == product.Name.ToLower())
                .FirstOrDefaultAsync();

            if (existingProduct == null)
            {
                _appDbScope.Products.Add(product);
                await _appDbScope.SaveChangesAsync();
                return new ServiceResponse() { Message = "Product added", Success = true };
            }

            return new ServiceResponse() { Message = "Product already exists", Success = false };
        }

        public async Task<ServiceResponse> DeleteProductAsync(int id)
        {
            var product = await _appDbScope.Products.FindAsync(id);
            if (product == null)
                return new ServiceResponse() { Message = "Product not found", Success = false };

            _appDbScope.Products.Remove(product);
            await _appDbScope.SaveChangesAsync();
            return new ServiceResponse() { Message = "Product deleted", Success = true };
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _appDbScope.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _appDbScope.Products.ToListAsync();
        }

        public async Task<ServiceResponse> UpdateProductAsync(Product product)
        {
            var existingProduct = await _appDbScope.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
                return new ServiceResponse() { Message = "Product not found", Success = false };

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Price = product.Price;
            existingProduct.Image = product.Image;
            existingProduct.CategoryId = product.CategoryId;

            await _appDbScope.SaveChangesAsync();
            return new ServiceResponse() { Message = "Product updated", Success = true };
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _appDbScope.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
