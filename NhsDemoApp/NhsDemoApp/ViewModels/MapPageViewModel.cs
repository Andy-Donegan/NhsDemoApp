using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Threading;
using Xamarin.Forms.BetterMaps;
using System.Linq;

namespace NhsDemoApp.ViewModels
{
    [QueryProperty(nameof(AppointmentId), nameof(AppointmentId))]
    public class MapPageViewModel : BaseViewModel
    {

        private string appointmentId;
        public Pin UserLocationPin { get; set; }
        public Pin ContactLocationPin { get; set; }
        public Map MyMap { get; private set; }
        public Button ContactLocationButton { get; set; }
        public Button ClearButton { get; set; }
        public Button MyLocationButton { get; set; }
        public Button OnSiteButton { get; set; }
        public Image CrossHair { get; set; }
        public Appointment Appointment { get; set; }
        public Xamarin.Essentials.Location LastKnownLocation { get; set; }
        public UserSettings UserSettings { get; set; }
        public bool FirstLoad { get; set; }

        public string AppointmentId
        {
            get
            {
                return appointmentId;
            }
            set
            {
                appointmentId = value;
                LoadAppointmentId(value);
            }
        }

        public async void LoadAppointmentId(string Id)
        {
            if (!await GetLastKnownLocation())
            {
                // failed create new location positions. ToDo.
            }

            try
            {
                Appointment = await DataStoreAppointment.GetAppointmentAsync(Id);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Load Item");
            }
            finally
            {
                UserSettings = await DataStoreUserSettings.GetUserSettingsAsync();
                if (Appointment.Latitude != 0 && Appointment.Longitude != 0)
                {
                    AddContactLocationPin(Appointment.Latitude, Appointment.Longitude);
                }

                UpdateMap(LastKnownLocation.Latitude, LastKnownLocation.Longitude);
                if (Appointment.OnSite == true)
                {
                    OnSiteButton.Text = "Leave\nsite";
                }
                else
                {
                    OnSiteButton.Text = "On\nSite";
                }
            }
        }

        public MapPageViewModel()
        {
            FirstLoad = true;
            Title = "Map Page";
            LastKnownLocation = new Xamarin.Essentials.Location();
            UserLocationPin = new Pin();
            ContactLocationPin = new Pin();
            CreateMap();
            CreateMapControls();
        }

        async Task<bool> GetLastKnownLocation()
        {            
            try
            {
                LastKnownLocation = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

                if (LastKnownLocation == null)
                {
                    if(!await RequestNewLocation())
                    {
                        return false;
                    }
                }
            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await App.Current.MainPage.DisplayAlert("Alert", "You have been alerted", "OK");
            }
            catch (Xamarin.Essentials.FeatureNotEnabledException fneEx)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "GPS Turned Off", "OK");
                // Handle not enabled on device exception
            }
            catch (Xamarin.Essentials.PermissionException pEx)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "App Requires Permission to access GPS", "OK");
                // Handle permission exception
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Something else Happened", "OK");
                // Unable to get location
            }   
            return true;
        }

        async Task<bool> RequestNewLocation()
        {
            var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            CancellationTokenSource cts = new CancellationTokenSource();
            LastKnownLocation = await Xamarin.Essentials.Geolocation.GetLocationAsync(request, cts.Token);
            if (LastKnownLocation == null)
            {
                // TODO We failed to get any location data for user at all,Set Default Location.
                return false;
            }
            return true;
        }

        public void CreateMap()
        {
            MyMap = new Map
            {
                MapType = MapType.Hybrid,
                MoveToLastRegionOnLayoutChange = false,
                IsShowingUser = true
            };            
            MyMap.MapClicked += OnMapClicked;
        }

        public void CreateMapControls()
        {
            ContactLocationButton = new Button
            {
                CornerRadius = 35,
                FontSize = 8,
                Padding = 0,
                Text = "Add\nContact\nLocation"
            };
            ContactLocationButton.Clicked += AddContactLocation;

            ClearButton = new Button
            {
                CornerRadius = 35,
                FontSize = 8,
                Padding = 0,
                Text = "Clear\nContact\nLocation"
            };
            ClearButton.Clicked += RemoveContactLocationPin;

            MyLocationButton = new Button
            {
                CornerRadius = 35,
                FontSize = 8,
                Padding = 0,
                Text = "My\nLocation"
            };
            MyLocationButton.Clicked += MoveMapToUserLocation;

            OnSiteButton = new Button
            {
                CornerRadius = 35,
                Padding = 0,
                FontSize = 8
            };
            OnSiteButton.Clicked += RegisterUserAtSite;

            CrossHair = new Image
            {
                Source = "crosshair.png"
            };
        }

        async void RegisterUserAtSite(object sender, EventArgs e)
        {
            var pinCheck = await SecurityCheck();
            if (pinCheck == false)
            {
                return;
            }
            if (Appointment.OnSite == false)
            {
                Appointment.OnSite = true;
                OnSiteButton.Text = "Leave\nSite";
                UserSettings.OnSiteID = Appointment.Id;
            }
            else
            {
                Appointment.OnSite = false;
                OnSiteButton.Text = "On\nSite";
                UserSettings.OnSiteID = "";
            }
        }

        void RemoveContactLocationPin(object sender, EventArgs e)
        {
            Appointment.Latitude = 0;
            Appointment.Longitude = 0;

            RemovePin(ContactLocationPin);
        }

        void AddContactLocation(object sender, EventArgs e)
        {
            double latitude = MyMap.VisibleRegion.Center.Latitude;
            double longitude = MyMap.VisibleRegion.Center.Longitude;

            Appointment.Latitude = latitude;
            Appointment.Longitude = longitude;

            AddContactLocationPin(latitude, longitude);
        }

        async Task<bool> SecurityCheck()
        {
            string result = await App.Current.MainPage.DisplayPromptAsync("Security Check", "Please enter your 4 digit security pin.", cancel: "Cancel", accept: "Ok", maxLength: 4, keyboard: Keyboard.Numeric);
            
            if (result != UserSettings.SecurityPin.ToString())
            {
                await App.Current.MainPage.DisplayAlert("Alert", "You entered : " + result + " this is incorrect. Check Security Pin on Home Page.", "OK");
                return false;
            }
            return true;
        }

        async void MoveMapToUserLocation(object sender, EventArgs e)
        {
            await GetLastKnownLocation();

            UpdateMap(LastKnownLocation.Latitude, LastKnownLocation.Longitude);
        }

        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            UpdateMap(e.Position.Latitude, e.Position.Longitude);
        }

        async void UpdateMap(double latitude, double longitude)
        {
            double distance;
            Position position = new Position(latitude, longitude);
            try
            {
                distance = MyMap.VisibleRegion.Radius.Kilometers;
            }
            catch
            {
                distance = 0.40;
            }

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(distance));
            if(FirstLoad == false)
            {
                MyMap.MoveToRegion(mapSpan);
            }
            else
            {
                FirstLoad = false;
                await Task.Delay(300);
                MyMap.MoveToRegion(mapSpan);
            }
 
        }

        public void RemovePin(Pin pinName)
        {
            MyMap.Pins.Remove(pinName);
        }

        async void AddContactLocationPin(double latitude, double longitude)
        {
            
            RemovePin(ContactLocationPin);
            ContactLocationPin = new Pin
            {
                Position = new Position(latitude, longitude),
                Address = await ReverseGeocodeAddress(latitude, longitude),
                Label = Appointment.Contact
            };
            MyMap.Pins.Add(ContactLocationPin);
        }

        async Task<string> ReverseGeocodeAddress(double latitude, double longitude)
        {
            try
            {

                var placemarks = await Xamarin.Essentials.Geocoding.GetPlacemarksAsync(latitude, longitude);

                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var geocodeAddress = placemark.FeatureName + "," + placemark.Thoroughfare + "\n" + placemark.SubAdminArea + " , " + placemark.PostalCode;
                    return geocodeAddress;
                }
            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }
            return "";
        }
    }
}
