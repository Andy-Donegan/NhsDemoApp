using NhsDemoApp.Services;
using NhsDemoApp.Views;
using Plugin.LocalNotification;
using System;
using Xamarin.Forms;
using NhsDemoApp.ViewModels;

namespace NhsDemoApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<MockDataStoreAppointment>();
            DependencyService.Register<MockDataStoreUserSettings>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
