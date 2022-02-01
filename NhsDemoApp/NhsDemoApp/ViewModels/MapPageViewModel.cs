using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NhsDemoApp.ViewModels
{
    [QueryProperty(nameof(AppointmentId), nameof(AppointmentId))]
    public class MapPageViewModel : BaseViewModel
    {

        private string appointmentId;
        public Map MyMap { get; private set; }
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
            MyMap = new Map
            {
                MapType = MapType.Hybrid,
                IsShowingUser = true,
                MoveToLastRegionOnLayoutChange = false
            };
        }

        public async void LoadAppointmentId(string Id)
        {
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
                UpdateMap();
            }
        }

        public void UpdateMap()
        {
            Pin pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(Appointment.Latitude, Appointment.Longitude),
                Label = Appointment.Contact
            };
            Position position = new Position(Appointment.Latitude, Appointment.Longitude);

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.38));
            MyMap.Pins.Add(pin);
            MyMap.MoveToRegion(mapSpan);
        }

    }
}
