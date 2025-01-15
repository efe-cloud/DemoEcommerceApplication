using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Ecommerce.client.Messages;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using Microsoft.Maui.Controls;
using Serilog;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.client.ViewModels
{
    public partial class PaymentViewModel : ObservableValidator
    {
        private readonly ICartApiService _cartService;
        private readonly IOrderApiService _orderService;
        private readonly OrderDataService _orderDataService;
        private readonly ILogger<PaymentViewModel> _logger;

        public PaymentViewModel(
            ICartApiService cartService,
            IOrderApiService orderService,
            OrderDataService orderDataService,
            ILogger<PaymentViewModel> logger)
        {
            _cartService = cartService;
            _orderService = orderService;
            _orderDataService = orderDataService;
            _logger = logger;
            CartSummaryItems = new ObservableCollection<CartItem>();
            PaymentOptions = new List<string> { "Credit Card", "PayPal", "Bank Transfer" };

            ValidateAllProperties();
        }

        // Observable Properties with Validation
        [ObservableProperty]
        [Required(ErrorMessage = "Card number is required.")]
        [RegularExpression(@"^(4|5)\d{15}$", ErrorMessage = "Card number must start with 4 (Visa) or 5 (MasterCard) and be 16 digits.")]
        private string cardNumber;

        [ObservableProperty]
        [Required(ErrorMessage = "CCV is required.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CCV must be 3 or 4 digits.")]
        private string ccv;

        [ObservableProperty]
        [Required(ErrorMessage = "Expiration month is required.")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        private int expirationMonth;

        [ObservableProperty]
        [Required(ErrorMessage = "Expiration year is required.")]
        [Range(2023, 2100, ErrorMessage = "Year must be a valid future year.")]
        private int expirationYear;

        [ObservableProperty]
        [Required(ErrorMessage = "Billing address is required.")]
        private string billingAddress;

        [ObservableProperty]
        [Required(ErrorMessage = "Please select a payment option.")]
        private string paymentOption;

        [ObservableProperty]
        private bool isAgreementChecked;

        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private bool isProcessing;

        public ObservableCollection<CartItem> CartSummaryItems { get; }

        [ObservableProperty]
        private double totalAmount;

        public List<string> PaymentOptions { get; }

        public bool IsNotProcessing => !IsProcessing;

        partial void OnIsProcessingChanged(bool value)
        {
            OnPropertyChanged(nameof(IsNotProcessing));
        }

        [RelayCommand]
        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Initializing PaymentViewModel and loading cart items.");
                IsProcessing = true;

                var items = await _cartService.GetMyCartAsync();
                if (items == null || !items.Any())
                {
                    _logger.LogWarning("Cart is empty or failed to load.");
                    StatusMessage = "Your cart is empty. Please add items before checkout.";
                    return;
                }

                CartSummaryItems.Clear();
                foreach (var item in items)
                {
                    CartSummaryItems.Add(item);
                }

                TotalAmount = CalculateTotalAmount();
                _logger.LogInformation($"Loaded {CartSummaryItems.Count} cart items. Total Amount: {TotalAmount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize PaymentViewModel.");
                StatusMessage = $"Failed to load cart: {ex.Message}";
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private double CalculateTotalAmount()
        {
            double total = 0;
            foreach (var item in CartSummaryItems)
            {
                total += item.TotalPrice;
            }
            return total;
        }

        [RelayCommand(CanExecute = nameof(IsNotProcessing))]
        public async Task ConfirmPaymentAsync()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                if (!IsAgreementChecked)
                {
                    StatusMessage = "You must agree to the terms and conditions.";
                    _logger.LogWarning("Payment confirmation attempted without agreement.");
                    return;
                }

                _logger.LogInformation("Starting payment confirmation process.");

                var paymentDetails = new PaymentDetails
                {
                    CardNumber = CardNumber,
                    Ccv = Ccv,
                    ExpirationMonth = ExpirationMonth,
                    ExpirationYear = ExpirationYear,
                    BillingAddress = BillingAddress,
                    PaymentOption = PaymentOption
                };

                IsProcessing = true;
                StatusMessage = string.Empty;

                try
                {
                    int orderId = await _orderService.CreateOrderAsync(paymentDetails);
                    if (orderId > 0)
                    {
                        _logger.LogInformation($"Order created with ID: {orderId}. Confirming payment.");
                        bool paymentConfirmed = await _orderService.ConfirmPaymentAsync(orderId);

                        if (paymentConfirmed)
                        {
                            _logger.LogInformation($"Payment confirmed for Order ID: {orderId}.");
                            await _orderDataService.AddOrderAsync(orderId);
                            WeakReferenceMessenger.Default.Send(new NewOrderCreatedMessage(orderId));
                            await Shell.Current.GoToAsync(nameof(Views.Desktop.PaymentSuccessPage)); // ✅ Navigate to success page
                        }
                        else
                        {
                            _logger.LogWarning($"Payment confirmation failed for Order ID: {orderId}.");
                            StatusMessage = "Payment confirmation failed. Please try again.";
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Order creation failed.");
                        StatusMessage = "Order creation failed. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An exception occurred during payment confirmation.");
                    StatusMessage = $"An error occurred: {ex.Message}";
                }
                finally
                {
                    IsProcessing = false;
                }
            }
            else
            {
                StatusMessage = "Please correct the errors before proceeding.";
            }
        }
    }
}
