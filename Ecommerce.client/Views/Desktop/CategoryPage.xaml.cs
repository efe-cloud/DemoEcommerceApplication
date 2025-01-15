using Ecommerce.client.ViewModels;

namespace Ecommerce.client.Views.Desktop
{
    public partial class CategoryPage : ContentPage
    {
        private readonly CategoryViewModel _viewModel;

        public CategoryPage(CategoryViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
            Loaded += async (s, e) => await _viewModel.LoadCategoriesAsync();
        }
    }
}
