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
using Microsoft.Maui.Controls;

namespace Ecommerce.client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // Subscribe to unhandled exception events
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        /// <summary>
        /// Handles unhandled exceptions from non-UI threads.
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            // Log the exception using Serilog
            Log.Error(exception, "Unhandled exception occurred.");

            // Display a user-friendly message on the UI thread
            Application.Current.Dispatcher.Dispatch(async () =>
            {
                if (MainPage != null)
                {
                    await MainPage.DisplayAlert("Error", "An unexpected error occurred. Please restart the app.", "OK");
                }
            });

            // Optionally, you might want to close the application or perform other actions
        }

        /// <summary>
        /// Handles unobserved task exceptions.
        /// </summary>
        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = e.Exception;
            // Log the exception using Serilog
            Log.Error(exception, "Unobserved task exception occurred.");

            // Display a user-friendly message on the UI thread
            Application.Current.Dispatcher.Dispatch(async () =>
            {
                if (MainPage != null)
                {
                    await MainPage.DisplayAlert("Error", "An unexpected error occurred. Please restart the app.", "OK");
                }
            });

            // Mark the exception as observed to prevent the process from terminating
            e.SetObserved();
        }

        protected override void OnStart()
        {
            base.OnStart();
            // Optional: Initialize any additional services or perform startup tasks
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            // Optional: Handle when your app sleeps
        }

        protected override void OnResume()
        {
            base.OnResume();
            // Optional: Handle when your app resumes
        }

        // Removed the Dispose method
    }
}
