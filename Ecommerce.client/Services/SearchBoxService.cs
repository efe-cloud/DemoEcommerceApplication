using Ecommerce.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.client.Services
{
    public class SearchBoxService : ISearchBoxService
    {
        private readonly IProductService _productService;

        public SearchBoxService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            // Retrieve all products (or call a dedicated API if you have one).
            var allProducts = await _productService.GetProductsAsync();

            // If the search term is empty, return everything (or an empty list if preferred).
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return allProducts;
            }

            // Filter by product name (case-insensitive).
            return allProducts
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
