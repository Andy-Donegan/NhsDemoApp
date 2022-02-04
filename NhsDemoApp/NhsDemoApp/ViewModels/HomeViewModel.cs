using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using NhsDemoApp.Models;
using System.Threading.Tasks;
using Plugin.LocalNotification;

namespace NhsDemoApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public UserSettings UserSettings { get; set; }
        public Command LoadUserSettings { get; }
        private string firstName;
        private string lastName;
        private string organisation;
        private TimeSpan currentTime;
        private int securityPin;

        public int SecurityPin
        {
            get => securityPin;
            set
            {
                if(value.ToString().Length < 4)
                {

                }
                else
                {
                    // Require Data Validation
                    SetProperty(ref securityPin, value);
                    UserSettings.SecurityPin = value;
                }
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                SetProperty(ref firstName, value);
                UserSettings.FirstName = firstName;
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                SetProperty(ref lastName, value);
                UserSettings.LastName = lastName;
            }
        }

        public string Organisation
        {
            get => organisation;
            set
            {
                SetProperty(ref organisation, value);
                UserSettings.Organisation = organisation;
            }
        }
        public TimeSpan CurrentTime
        {
            get => currentTime;
            set
            {
                SetProperty(ref currentTime, value);
                UserSettings.CurrentTime = currentTime;
            }
        }

        public HomeViewModel()
        {
            Title = "Home";
            LoadUserSettings = new Command(async () => await GetUserSettings());
        }

        async Task GetUserSettings()
        {
            // ToDelete - Test Notification for new module
            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "Did you Fart",
                Description = "No it was definitely Taylor",
                ReturningData = "Dummy data", // Returning data when tapped on notification.
                Schedule =
    {
        NotifyTime = DateTime.Now.AddSeconds(10) // Used for Scheduling local notification, if not specified notification will show immediately.
    }
            };
            await NotificationCenter.Current.Show(notification);
            IsBusy = true;
            try
            {
                UserSettings = await DataStoreUserSettings.GetUserSettingsAsync();
                FirstName = UserSettings.FirstName;
                LastName = UserSettings.LastName;
                Organisation = UserSettings.Organisation;
                CurrentTime = UserSettings.CurrentTime;
                SecurityPin = UserSettings.SecurityPin;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Load User Settings.");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}


