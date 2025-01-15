using Ecommerce.library.Models;
using Ecommerce.library.Responses;

namespace ECommerce.Api.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse> RegisterAsync(string email, string password);
        Task<string> LoginAsync(string email, string password);
        
        Task<User> GetUserByEmailAsync(string email);
    }
}
