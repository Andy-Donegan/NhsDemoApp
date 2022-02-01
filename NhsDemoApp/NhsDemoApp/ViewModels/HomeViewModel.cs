using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using NhsDemoApp.Models;
using System.Threading.Tasks;

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

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        public string Organisation
        {
            get => organisation;
            set => SetProperty(ref organisation, value);
        }
        public TimeSpan CurrentTime
        {
            get => currentTime;
            set => SetProperty(ref currentTime, value);
        }

        public HomeViewModel()
        {
            Title = "Home";
            LoadUserSettings = new Command(async () => await GetUserSettings());
        }

        async Task GetUserSettings()
        {
            IsBusy = true;
            try
            {
                UserSettings = await DataStoreUserSettings.GetUserSettingsAsync();
                FirstName = UserSettings.FirstName;
                LastName = UserSettings.LastName;
                Organisation = UserSettings.Organisation;
                CurrentTime = UserSettings.CurrentTime;
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


