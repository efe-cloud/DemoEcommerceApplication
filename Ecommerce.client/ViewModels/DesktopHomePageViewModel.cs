using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using Ecommerce.client.Views.Desktop;
using Microsoft.Maui.Controls;

namespace Ecommerce.client.ViewModels
{
    public partial class DesktopHomePageViewModel : BaseViewModel
    {
        private readonly ISearchBoxService _searchBoxService;

        public DesktopHomePageViewModel(ISearchBoxService searchBoxService)
        {
            _searchBoxService = searchBoxService;
            SearchResults = new ObservableCollection<Product>();

            SearchCommand = new AsyncRelayCommand(OnSearchAsync);
            SelectProductCommand = new AsyncRelayCommand<Product>(SelectProductAsync);
            NavigateToProductsCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync(nameof(ProductsViewPage)));
            NavigateToCartCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync(nameof(CartPage)));
            NavigateToAccountCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync(nameof(AccountPage)));
            NavigateToCategoriesCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync(nameof(CategoryPage)));
            NavigateToOrdersCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync(nameof(OrderHistoryPage)));
            NavigateToPaymentCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync(nameof(PaymentPage)));
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public ObservableCollection<Product> SearchResults { get; }

        public bool HasSearchResults => SearchResults.Count > 0;

        public ICommand SearchCommand { get; }
        public ICommand SelectProductCommand { get; }
        public ICommand NavigateToProductsCommand { get; }
        public ICommand NavigateToCartCommand { get; }
        public ICommand NavigateToAccountCommand { get; }
        public ICommand NavigateToCategoriesCommand { get; }
        public ICommand NavigateToOrdersCommand { get; }
        public ICommand NavigateToPaymentCommand { get; }

        private async Task OnSearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                await Shell.Current.CurrentPage.DisplayAlert("Info", "Please enter a search term.", "OK");
                return;
            }

            try
            {
                var products = await _searchBoxService.SearchProductsAsync(SearchQuery);
                SearchResults.Clear();
                foreach (var product in products)
                {
                    SearchResults.Add(product);
                }
                OnPropertyChanged(nameof(HasSearchResults));

                if (products.Count == 0)
                {
                    await Shell.Current.CurrentPage.DisplayAlert("No Results", "No products found matching your search.", "OK");
                }
            }
            catch (Exception)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Error", "An error occurred while searching. Please try again.", "OK");
            }
        }

        private async Task SelectProductAsync(Product product)
        {
            if (product == null) return;
            await Shell.Current.GoToAsync($"{nameof(Views.Desktop.ProductDetailPage)}?productId={product.Id}");
        }
    }
}
