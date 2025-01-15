using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ecommerce.client.Services;
using System.Threading.Tasks;

namespace Ecommerce.client.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly SessionService _sessionService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string statusMessage;

        public LoginViewModel(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                await _sessionService.LoginAsync(Email, Password);

                if (_sessionService.IsAuthenticated)
                {
                    StatusMessage = "Login successful!";
                    await Shell.Current.GoToAsync(".."); // Navigate back to the previous page
                }
                else
                {
                    StatusMessage = "Invalid credentials";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"ERROR: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }
    }
}
