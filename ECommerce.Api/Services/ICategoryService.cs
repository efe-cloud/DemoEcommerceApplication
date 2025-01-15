using Ecommerce.library.Models;

namespace Ecommerce.Api.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
    }
}
