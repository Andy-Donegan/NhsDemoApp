using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading;

namespace NhsDemoApp.ViewModels
{
    [QueryProperty(nameof(AppointmentId), nameof(AppointmentId))]
    public class MapPageViewModel : BaseViewModel
    {

        private string appointmentId;
        public Map MyMap { get; private set; }
        public Button MyButton { get; set; }
        public Appointment Appointment { get; set; }

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

        public MapPageViewModel()
        {
            Title = "Map Page";

            MyMap = new Map
            {
                MapType = MapType.Hybrid,
                //IsShowingUser = true,
                MoveToLastRegionOnLayoutChange = false
            };
            MyMap.MapClicked += OnMapClicked;

            MyButton = new Button
            {
                CornerRadius = 50,
                FontSize = 8,
                ImageSource="icon_about"                
            };
            //MyButton.Clicked += NewFunctionToWrite.
        }
        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            UpdateMap(e.Position.Latitude, e.Position.Longitude);
        }

        public async void LoadAppointmentId(string Id)
        {
            Xamarin.Essentials.Location lastKnownLocation = new Xamarin.Essentials.Location();
            string result = await App.Current.MainPage.DisplayPromptAsync("Security Check", "Please enter your 4 digit pin.", cancel:"No Idea", accept:"Enter", maxLength: 4, keyboard: Keyboard.Numeric);
            if (result == null || result == ""){
                result = "fuck all";
            }
            await App.Current.MainPage.DisplayAlert("Alert", "You entered : " + result, "OK");
            try
            {
                lastKnownLocation = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

                if (lastKnownLocation == null)
                {
                    var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    CancellationTokenSource cts = new CancellationTokenSource();
                    lastKnownLocation = await Xamarin.Essentials.Geolocation.GetLocationAsync(request, cts.Token);
                    if (lastKnownLocation == null)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Something else Happened", "OK");
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
                UpdateMap(lastKnownLocation.Latitude, lastKnownLocation.Longitude);
                //UpdateMap(Appointment.Latitude, Appointment.Longitude);
            }
        }

        public void UpdateMap(double latitude, double longitude)
        {
            MyMap.Pins.Clear();
            Pin pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(latitude, longitude),
                Label = Appointment.Contact
            };
            Position position = new Position(latitude, longitude);

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.38));
            MyMap.Pins.Add(pin);
            MyMap.MoveToRegion(mapSpan);
        }

    }
}
