using Ecommerce.client.ViewModels;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Ecommerce.client.Views.Desktop
{
    public partial class SplashPage : ContentPage
    {
        private readonly SplashViewModel _viewModel;

        public SplashPage(SplashViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;

            // Subscribe to ViewModel events BEFORE initialization
            _viewModel.FadeInRequested += OnFadeInRequested;
            _viewModel.FadeOutRequested += OnFadeOutRequested;
            _viewModel.NavigationRequested += OnNavigationRequested;
            

            // Now, initialize the ViewModel
            _viewModel.Initialize();
        }

        private async void OnFadeInRequested()
        {
            await SplashImage.FadeTo(1, 1000); // Fade in over 1 second
        }

        private async void OnFadeOutRequested()
        {
            await SplashImage.FadeTo(0, 1000); // Fade out over 1 second
        }

        private async Task OnNavigationRequested()
        {
            // Navigate to DesktopHomePage and remove SplashPage from the navigation stack
            await Shell.Current.GoToAsync("DesktopHomePage");
        }
    }
}
