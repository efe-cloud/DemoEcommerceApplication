using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.client.Models;

namespace Ecommerce.client.Services
{
    public interface ISearchBoxService
    {
        /// <summary>
        /// Searches products by the given search term.
        /// </summary>
        /// <param name="searchTerm">The text to search on product properties, e.g. product name.</param>
        /// <returns>A list of products matching the search term.</returns>
        Task<List<Product>> SearchProductsAsync(string searchTerm);
    }
}
