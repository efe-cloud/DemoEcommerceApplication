using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using MvvmHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Microsoft.Maui.Controls;
using Ecommerce.client.Views.Desktop;
using Ecommerce.client.Messages;
using CommunityToolkit.Mvvm.Messaging; 

namespace Ecommerce.client.ViewModels
{
    public partial class ProductsViewViewModel : BaseViewModel, IRecipient<NewOrderCreatedMessage>
    {
        private readonly IProductService _productService;
        private readonly ICartApiService _cartApiService;
        private readonly ISearchBoxService _searchBoxService;
        private readonly SessionService _sessionService;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private ObservableRangeCollection<ProductTem> products = new();

        [ObservableProperty]
        private ObservableRangeCollection<ProductTem> searchResults = new();

        public ICommand SearchCommand => new AsyncRelayCommand(OnSearch);

        public ProductsViewViewModel(
            IProductService productService,
            ICartApiService cartApiService,
            ISearchBoxService searchBoxService,
            SessionService sessionService)
        {
            _productService = productService;
            _cartApiService = cartApiService;
            _searchBoxService = searchBoxService;
            _sessionService = sessionService;

            Title = "Available Products";
            GetProducts();

            // Register to receive NewOrderCreatedMessage
            WeakReferenceMessenger.Default.Register<NewOrderCreatedMessage>(this);
        }

        private async void GetProducts()
        {
            var items = await _productService.GetProductsAsync();
            if (items == null) return;

            Products.Clear();
            foreach (var p in items)
            {
                var temp = new ProductTem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    CategoryId = p.CategoryId
                };

                if (!string.IsNullOrEmpty(p.Image))
                {
                    temp.Image = ImageSource.FromFile(p.Image);
                }

                Products.Add(temp);
            }

            // Initialize SearchResults with all
            SearchResults.ReplaceRange(Products);
        }

        private async Task OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // Show all if empty
                SearchResults.ReplaceRange(Products);
                return;
            }

            var res = await _searchBoxService.SearchProductsAsync(SearchQuery);
            if (res == null)
            {
                SearchResults.Clear();
                return;
            }

            // Convert Product -> ProductTem
            var mapped = res.Select(p => new ProductTem
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                CategoryId = p.CategoryId,
                Image = string.IsNullOrEmpty(p.Image) ? null : ImageSource.FromFile(p.Image)
            }).ToList();

            SearchResults.ReplaceRange(mapped);
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
        public async Task SelectProduct(ProductTem product)
        {
            if (product == null) return;
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?productId={product.Id}");
        }

        // Implementation of IRecipient<NewOrderCreatedMessage>
        public async void Receive(NewOrderCreatedMessage message)
        {
            // Refresh the product list or perform any necessary updates
            await GetProductsAsync();
        }

        private async Task GetProductsAsync()
        {
            // Similar logic to GetProducts(), but as a Task
            var items = await _productService.GetProductsAsync();
            if (items == null) return;

            Products.Clear();
            foreach (var p in items)
            {
                var temp = new ProductTem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    CategoryId = p.CategoryId
                };

                if (!string.IsNullOrEmpty(p.Image))
                {
                    temp.Image = ImageSource.FromFile(p.Image);
                }

                Products.Add(temp);
            }

            // Initialize SearchResults with all
            SearchResults.ReplaceRange(Products);
        }
    }
}
