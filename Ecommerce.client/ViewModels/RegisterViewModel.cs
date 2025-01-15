using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using Ecommerce.client.Services;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;

namespace Ecommerce.client.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAuthApiService _authApiService;
        private readonly SessionService _sessionService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isBusy; // For loading indicator

        public RegisterViewModel(IAuthApiService authApiService, SessionService sessionService)
        {
            _authApiService = authApiService;
            _sessionService = sessionService;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Passwords do not match";
                    return;
                }

                var registrationResult = await _authApiService.RegisterAsync(Email, Password);
                if (registrationResult)
                {
                    // Attempt to log in the user automatically
                    var loginResult = await _sessionService.LoginAsync(Email, Password);
                    if (loginResult)
                    {
                        // Show success toast
                        var toast = Toast.Make("Registration and login successful!", ToastDuration.Short, 14);
                        await toast.Show();

                        // Navigate back to the previous page
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        ErrorMessage = "Registration successful, but automatic login failed. Please log in manually.";
                        await Shell.Current.GoToAsync("LoginPage");
                    }
                }
                else
                {
                    ErrorMessage = "Registration failed. Email may be taken.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
