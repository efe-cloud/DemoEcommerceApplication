using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ecommerce.client.Models;
using System.Threading.Tasks;

namespace Ecommerce.client.Services
{
    public partial class SessionService : ObservableObject
    {
        private readonly IAuthApiService _authApiService;

        [ObservableProperty]
        private bool isAuthenticated;

        [ObservableProperty]
        private string userEmail;

        public SessionService(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        public async Task CheckLoginStatusAsync()
        {
            try
            {
                var loggedIn = await _authApiService.IsLoggedInAsync();
                if (loggedIn)
                {
                    var currentUser = await _authApiService.GetCurrentUserAsync();
                    if (currentUser != null)
                    {
                        IsAuthenticated = true;
                        UserEmail = currentUser.Email;
                        OnPropertyChanged(nameof(IsAuthenticated));
                        OnPropertyChanged(nameof(UserEmail));
                        Console.WriteLine($"[SessionService] User logged in: {UserEmail}");
                        return;
                    }
                }
                IsAuthenticated = false;
                UserEmail = string.Empty;
                Console.WriteLine("[SessionService] No valid session found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SessionService] ERROR: {ex.Message}");
            }
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var token = await _authApiService.LoginAsync(email, password);
                if (!string.IsNullOrEmpty(token))
                {
                    await CheckLoginStatusAsync();
                    Console.WriteLine("[SessionService] Login successful, session updated.");
                    return true;
                }
                else
                {
                    Console.WriteLine("[SessionService] Login failed, token is null.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SessionService] Login error: {ex.Message}");
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _authApiService.LogoutAsync();
                IsAuthenticated = false;
                UserEmail = string.Empty;
                OnPropertyChanged(nameof(IsAuthenticated));
                OnPropertyChanged(nameof(UserEmail));
                Console.WriteLine("[SessionService] User logged out.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SessionService] Logout error: {ex.Message}");
            }
        }
    }
}
