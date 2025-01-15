using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using CommunityToolkit.Mvvm.Messaging;
using Ecommerce.client.Messages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Ecommerce.client.Views.Desktop;

namespace Ecommerce.client.ViewModels
{
    [QueryProperty(nameof(ProductId), "productId")]
    public partial class ProductDetailPageViewModel : BaseViewModel, IRecipient<NewOrderCreatedMessage>
    {
        private readonly IProductService _productService;
        private readonly ICartApiService _cartApiService;
        private readonly SessionService _sessionService;

        [ObservableProperty]
        private ProductTem product;

        private int productId;
        public int ProductId
        {
            get => productId;
            set
            {
                SetProperty(ref productId, value);
                LoadProduct(); // Once ID is set
            }
        }

        public ProductDetailPageViewModel(
            IProductService productService,
            ICartApiService cartApiService,
            SessionService sessionService)
        {
            _productService = productService;
            _cartApiService = cartApiService;
            _sessionService = sessionService;

            // Register to receive NewOrderCreatedMessage
            WeakReferenceMessenger.Default.Register<NewOrderCreatedMessage>(this);
        }

        private async void LoadProduct()
        {
            var all = await _productService.GetProductsAsync();
            var p = all.FirstOrDefault(x => x.Id == ProductId);
            if (p != null)
            {
                Product = new ProductTem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    CategoryId = p.CategoryId,
                    Image = string.IsNullOrEmpty(p.Image) ? null : ImageSource.FromFile(p.Image)
                };
            }
        }

        [RelayCommand]
        public async Task AddToCartAsync(ProductTem productTem) // Accept ProductTem as parameter
        {
            if (!_sessionService.IsAuthenticated)
            {
                bool shouldLogin = await Shell.Current.DisplayAlert(
                    "Authentication Required",
                    "You need to be logged in to add items to your cart. Would you like to log in now?",
                    "Yes",
                    "No");

                if (shouldLogin)
                {
                    // Navigate to the Login Page
                    await Shell.Current.GoToAsync(nameof(LoginPage));
                }

                return;
            }

            if (productTem == null)
            {
                await Shell.Current.DisplayAlert("Error", "Product information is missing.", "OK");
                return;
            }

            try
            {
                // Proceed to add the product to the cart
                await _cartApiService.AddToCartAsync(productTem.Id, 1); // Assuming quantity 1
                await Shell.Current.DisplayAlert("Success", $"{productTem.Name} added to cart.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to add to cart: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task SelectProduct(ProductTem productTem)
        {
            if (productTem == null) return;
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?productId={productTem.Id}");
        }

        // Implementation of IRecipient<NewOrderCreatedMessage>
        public async void Receive(NewOrderCreatedMessage message)
        {
            // Handle new order if necessary
            // For example, refresh product details or related data
            await Task.CompletedTask;
        }
    }
}
