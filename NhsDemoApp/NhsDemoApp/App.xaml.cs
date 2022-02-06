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

            NotificationCenter.NotificationLog += NotificationCenter_NotificationLog;
            NotificationCenter.Current.NotificationTapped += LoadPageFromNotification;
        }

        private void NotificationCenter_NotificationLog(NotificationLogArgs e)
        {
            //TODO remove after testing.
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Error);
        }

        private async void LoadPageFromNotification(NotificationEventArgs e)
        {
            if (e.Request is null)
            {
                return;
            }
            
            var appointmentId = e.Request.ReturningData;
          
            await Shell.Current.GoToAsync($"{nameof(AppointmentDetailPage)}?{nameof(AppointmentDetailViewModel.AppointmentId)}={appointmentId}");
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
