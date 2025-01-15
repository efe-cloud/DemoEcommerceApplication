using System.Threading.Tasks;
using Ecommerce.client.Models;

namespace Ecommerce.client.Services
{
    public interface IAuthApiService
    {
        Task<bool> RegisterAsync(string email, string password);
        Task<string> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<bool> IsLoggedInAsync();
        Task<UserDto> GetCurrentUserAsync();
    }
}
