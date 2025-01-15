using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Ecommerce.client.Messages;
using Microsoft.Maui.Controls;
using Ecommerce.client.Views.Desktop;

namespace Ecommerce.client.ViewModels
{
    public partial class AccountViewModel : ObservableObject, IRecipient<NewOrderCreatedMessage>
    {
        private readonly SessionService _sessionService;
        private readonly IOrderApiService _orderApiService;
        private readonly OrderDataService _orderDataService;

        [ObservableProperty]
        private bool isAuthenticated;

        [ObservableProperty]
        private string userEmail;

        private ObservableCollection<UserOrder> myOrders = new();
        public ObservableCollection<UserOrder> MyOrders
        {
            get => myOrders;
            set => SetProperty(ref myOrders, value);
        }

        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand NavigateToOrderHistoryCommand { get; } 

        public AccountViewModel(SessionService sessionService, IOrderApiService orderApiService, OrderDataService orderDataService)
        {
            _sessionService = sessionService;
            _orderApiService = orderApiService;
            _orderDataService = orderDataService;

            NavigateToLoginCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync("LoginPage"));
            NavigateToRegisterCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync("RegisterPage"));
            LogoutCommand = new AsyncRelayCommand(LogoutAsync);
            NavigateToOrderHistoryCommand = new AsyncRelayCommand(NavigateToOrderHistoryAsync); 

            _sessionService.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(_sessionService.IsAuthenticated) ||
                    e.PropertyName == nameof(_sessionService.UserEmail))
                {
                    IsAuthenticated = _sessionService.IsAuthenticated;
                    UserEmail = _sessionService.UserEmail;

                    if (IsAuthenticated)
                    {
                        await RefreshOrdersAsync();
                    }
                    OnPropertyChanged(nameof(IsAuthenticated));
                    OnPropertyChanged(nameof(UserEmail));
                }
            };

            CheckLoginStatusAsync();

            // Register to receive NewOrderCreatedMessage
            WeakReferenceMessenger.Default.Register<NewOrderCreatedMessage>(this);
        }

        [RelayCommand]
        private async Task RefreshOrdersAsync()
        {
            if (!IsAuthenticated) return;
            var orders = await _orderApiService.GetMyOrdersAsync();
            MyOrders = new ObservableCollection<UserOrder>(orders);
        }

        private async void CheckLoginStatusAsync()
        {
            await _sessionService.CheckLoginStatusAsync();
            IsAuthenticated = _sessionService.IsAuthenticated;
            UserEmail = _sessionService.UserEmail;
            if (IsAuthenticated)
            {
                await RefreshOrdersAsync();
            }
        }

        private async Task LogoutAsync()
        {
            await _sessionService.LogoutAsync();
            IsAuthenticated = false;
            UserEmail = string.Empty;
            MyOrders.Clear();
        }

        private async Task NavigateToOrderHistoryAsync()
        {
            await Shell.Current.GoToAsync(nameof(OrderHistoryPage));
        }

        public async void Receive(NewOrderCreatedMessage message)
        {
            await RefreshOrdersAsync();
        }
    }
}
