using System;
using System.Threading.Tasks;

namespace Ecommerce.client.ViewModels
{
    public class SplashViewModel
    {
        public event Action FadeInRequested;
        public event Action FadeOutRequested;
        public event Func<Task> NavigationRequested;

        public SplashViewModel()
        {
            
        }

        public void Initialize()
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            // Trigger Fade-In Animation
            FadeInRequested?.Invoke();

            // Wait for fade-in to complete (1 second)
            await Task.Delay(1000);

            // Simulate some initialization tasks
            await Task.Delay(2000);

            // Trigger Fade-Out Animation
            FadeOutRequested?.Invoke();

            // Wait for fade-out to complete (1 second)
            await Task.Delay(1000);

            // Navigate to DesktopHomePage
            if (NavigationRequested != null)
            {
                await NavigationRequested.Invoke();
            }
        }
    }
}
