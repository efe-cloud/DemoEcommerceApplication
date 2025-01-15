using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using Ecommerce.client.Views.Desktop;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Ecommerce.client.ViewModels
{
    public partial class CategoryViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly ICartApiService _cartApiService;
        private readonly SessionService _sessionService;

        // Holds all categories fetched from the service
        public ObservableRangeCollection<Category> AllCategories { get; } = new ObservableRangeCollection<Category>();

        // Holds categories to display (can be filtered)
        public ObservableRangeCollection<Category> Categories { get; } = new ObservableRangeCollection<Category>();

        // Holds products of the selected category
        public ObservableRangeCollection<Product> Products { get; } = new ObservableRangeCollection<Product>();

        // Indicates if a category is selected
        [ObservableProperty]
        private bool isCategorySelected;

        // Controls visibility of the Categories section
        public bool AreCategoriesVisible => !IsCategorySelected;

        // Controls visibility of the Products section
        public bool AreProductsVisible => IsCategorySelected;

        // Flag to prevent multiple initializations
        private bool _isInitialized = false;

        public CategoryViewModel(IProductService productService, ICartApiService cartApiService, SessionService sessionService)
        {
            _productService = productService;
            _cartApiService = cartApiService;
            _sessionService = sessionService;
        }

        /// <summary>
        /// Called when the view appears to initialize data.
        /// Ensure this is called only once.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;
            await LoadCategoriesAsync();
        }

        /// <summary>
        /// Called when IsCategorySelected changes to update dependent properties
        /// </summary>
        partial void OnIsCategorySelectedChanged(bool value)
        {
            // Notify the UI that AreCategoriesVisible and AreProductsVisible have changed
            OnPropertyChanged(nameof(AreCategoriesVisible));
            OnPropertyChanged(nameof(AreProductsVisible));
        }

        /// <summary>
        /// Loads all categories from the product service.
        /// </summary>
        public async Task LoadCategoriesAsync()
        {
            try
            {
                var cats = await _productService.GetCategoriesAsync();

                // Remove duplicates if any
                var distinctCats = cats?.GroupBy(c => c.Id).Select(g => g.First()).ToList();

                if (distinctCats?.Count > 0)
                {
                    AllCategories.ReplaceRange(distinctCats);
                    Categories.ReplaceRange(AllCategories);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Command to select a category.
        /// Filters the Categories to show only the selected one and loads its products.
        /// </summary>
        [RelayCommand]
        public async Task SelectCategory(Category category)
        {
            if (category == null) return;

            // Set the flag indicating a category is selected
            IsCategorySelected = true;

            // Clear and set Categories to only the selected one
            Categories.ReplaceRange(AllCategories.Where(c => c.Id == category.Id));

            // Load products for the selected category
            await LoadProductsAsync(category.Id);
        }

        /// <summary>
        /// Loads products based on the selected category ID.
        /// </summary>
        /// <param name="categoryId">The ID of the selected category.</param>
        private async Task LoadProductsAsync(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAsync(categoryId);

                // Remove duplicates if any
                var distinctProducts = products?.GroupBy(p => p.Id).Select(g => g.First()).ToList();

                Products.Clear();
                if (distinctProducts?.Count > 0)
                {
                    Products.AddRange(distinctProducts);
                }
                else
                {
                    await Shell.Current.DisplayAlert("No Products", "There are no products in this category.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Command to reset the view back to showing all categories.
        /// </summary>
        [RelayCommand]
        public void ResetCategories()
        {
            // Reset the flag indicating no category is selected
            IsCategorySelected = false;

            // Restore the full list of categories
            Categories.ReplaceRange(AllCategories);

            // Clear the products list
            Products.Clear();
        }

        /// <summary>
        /// Command to add a product to the cart.
        /// </summary>
        /// <param name="product">The product to add.</param>
        [RelayCommand]
        public async Task AddToCartAsync(Product product)
        {
            if (product == null) return;

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
                await _cartApiService.AddToCartAsync(product.Id, 1); // Assuming quantity 1
                await Shell.Current.DisplayAlert("Success", $"{product.Name} added to cart.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to add to cart: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Command to select a product and navigate to its detail page.
        /// </summary>
        /// <param name="product">The selected product.</param>
        [RelayCommand]
        public async Task SelectProduct(Product product)
        {
            if (product == null) return;
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?productId={product.Id}");
        }
    }
}
