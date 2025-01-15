using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ecommerce.client.Models;
using Ecommerce.client.Services;
using CommunityToolkit.Maui.Alerts;
using MvvmHelpers;
using Microsoft.Maui.Storage;
using System.IO;
using System.Threading.Tasks;

namespace Ecommerce.client.ViewModels
{
    public partial class AddProductPageViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Category> Categories { get; set; } = new();

        [ObservableProperty]
        private Product _addProductModel;

        [ObservableProperty]
        private string _selectedImageName;

        [ObservableProperty]
        private Category _selectedItem;

        private readonly IProductService _productService;

        public AddProductPageViewModel(IProductService productService)
        {
            Title = "Add Product";
            AddProductModel = new Product();
            _productService = productService;
            LoadCategories();
        }

        [RelayCommand]
        public async Task AddImage()
        {
            var image = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select Product Image",
                FileTypes = FilePickerFileType.Images
            });

            if (image is null)
                return;

            // Extract the image file name (e.g., "laptop.png")
            var imageName = Path.GetFileName(image.FullPath);
            AddProductModel.Image = imageName; // Assign only the image name
            SelectedImageName = $"Resources/Images/{imageName}"; // Ensure correct path for preview
        }

        [RelayCommand]
        public async Task SaveProductData()
        {
            if (SelectedItem is null)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please select a category.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(AddProductModel.Name))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please enter the product name.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(AddProductModel.Image))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please add an image for the product.", "OK");
                return;
            }

            AddProductModel.CategoryId = SelectedItem.Id;

            var result = await _productService.AddProductAsync(AddProductModel);
            if (result.Success)
            {
                await Shell.Current.DisplayAlert("Success", result.Message, "OK");
                // Optionally, clear the form or navigate away
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", result.Message, "OK");
            }
        }

        private async void LoadCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            if (categories is null)
                return;

            if (Categories.Count > 0)
                Categories.Clear();

            foreach (var category in categories)
                Categories.Add(category);
        }
    }
}
