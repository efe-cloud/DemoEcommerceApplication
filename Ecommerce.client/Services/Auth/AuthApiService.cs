
using Ecommerce.client.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Ecommerce.client.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Base address can be set in MauiProgram if desired
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var payload = new { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", payload);

            return response.IsSuccessStatusCode;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var payload = new { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", payload);

            if (!response.IsSuccessStatusCode) return null;
            var token = await response.Content.ReadAsStringAsync();

            // Save token to SecureStorage
            await SecureStorage.Default.SetAsync("auth_token", token);

            // Also attach the token to the HttpClient for future requests
            _httpClient.DefaultRequestHeaders.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return token;
        }

        public async Task<bool> LogoutAsync()
        {
            // Just remove from SecureStorage
            SecureStorage.Default.Remove("auth_token");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return await Task.FromResult(true);
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await SecureStorage.Default.GetAsync("auth_token");
            return !string.IsNullOrEmpty(token);
        }
        public async Task<UserDto> GetCurrentUserAsync()
        {
            // We must have the JWT in the Authorization header 
            // (set in LoginAsync) or from SecureStorage retrieval
            var response = await _httpClient.GetAsync("api/Auth/me");
            if (!response.IsSuccessStatusCode)
                return null; // e.g., not logged in or token invalid

            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            return user;
        }
        
    }
}
