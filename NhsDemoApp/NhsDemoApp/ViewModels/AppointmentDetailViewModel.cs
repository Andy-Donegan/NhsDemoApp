using NhsDemoApp.Models;
using NhsDemoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NhsDemoApp.ViewModels
{
    [QueryProperty(nameof(AppointmentId), nameof(AppointmentId))]
    public class AppointmentDetailViewModel : BaseViewModel
    {
        private string appointmentId;
        public ObservableCollection<Appointment> Appointments { get; }
        public Command LoadAppointmentCommand { get; }
        public Command LoadMapCommand { get; }

        public AppointmentDetailViewModel()
        {
            Title = "Appointment Details";
            Appointments = new ObservableCollection<Appointment>();
            LoadAppointmentCommand = new Command(async () => await ExecuteLoadAppointmentCommand());

            LoadMapCommand = new Command(OnLoadMap);
        }
        public string AppointmentId
        {
            get
            {
                return appointmentId;
            }
            set
            {
                appointmentId = value;
            }
        }

        async Task ExecuteLoadAppointmentCommand()
        {
            IsBusy = true;

            try
            {
                Appointments.Clear();
                var appointments = await DataStoreAppointment.GetAppointmentsAsync(true);
                foreach (Appointment appointment in appointments)
                {
                    if (appointment.Id == appointmentId)
                    {
                        Appointments.Add(appointment);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
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

        private async void OnLoadMap(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(MapPage)}?{nameof(MapPageViewModel.AppointmentId)}={AppointmentId}");
        }

    }
}
