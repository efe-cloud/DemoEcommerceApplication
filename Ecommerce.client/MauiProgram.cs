using CommunityToolkit.Maui;
using Ecommerce.client.Services;
using Ecommerce.client.ViewModels;
using Ecommerce.client.Views.Desktop;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Licensing;
using MvvmHelpers;
using Serilog;
using Serilog.Extensions.Logging; 

namespace Ecommerce.client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
               .WriteTo.Debug()
               .CreateLogger();

            var builder = MauiApp.CreateBuilder();

            
            SyncfusionLicenseProvider.RegisterLicense("");

            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(); 

            RegisterViewModels(builder.Services);
            RegisterPages(builder.Services);
            RegisterServices(builder.Services);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            
            services.AddTransient<DesktopHomePageViewModel>();
            services.AddTransient<AddProductPageViewModel>();
            services.AddTransient<ProductsViewViewModel>();
            services.AddTransient<CategoryViewModel>();

            
            services.AddTransient<CartViewModel>();
            services.AddTransient<ProductDetailPageViewModel>();
            services.AddTransient<PaymentViewModel>();
            services.AddTransient<PaymentSuccessViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<AccountViewModel>();
            services.AddTransient<OrderHistoryViewModel>();
            services.AddTransient<SplashViewModel>();
        }

        private static void RegisterPages(IServiceCollection services)
        {
            services.AddTransient<DesktopHomePage>();
            services.AddTransient<AddProductPage>();
            services.AddTransient<ProductsViewPage>();
            services.AddTransient<CategoryPage>();

            services.AddTransient<CartPage>();
            services.AddTransient<ProductDetailPage>();
            services.AddTransient<PaymentPage>();
            services.AddTransient<PaymentSuccessPage>();
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<AccountPage>();
            services.AddTransient<OrderHistoryPage>();
            services.AddTransient<SplashPage>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            //
            // Create a SINGLE HttpClient for the entire client app
            //
            services.AddSingleton(sp =>
            {
                return new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:7235") // Use your HTTPS port
                };
            });

            //
            // Register your services to use that single HttpClient
            //
            services.AddSingleton<IAuthApiService, AuthApiService>();
            services.AddSingleton<SessionService>();

            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<ICartApiService, CartApiService>();
            services.AddSingleton<IOrderApiService, OrderApiService>();

            
            services.AddSingleton<ISearchBoxService, SearchBoxService>();
            services.AddSingleton<OrderDataService>();
        }
    }
}

//hist reset was fot hiding keys from commit history 