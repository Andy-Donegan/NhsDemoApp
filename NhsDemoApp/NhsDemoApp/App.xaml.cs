using NhsDemoApp.Services;
using NhsDemoApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
