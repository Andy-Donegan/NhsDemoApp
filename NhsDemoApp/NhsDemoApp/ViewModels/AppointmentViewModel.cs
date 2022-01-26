using NhsDemoApp.Models;
using NhsDemoApp.Services;
using NhsDemoApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NhsDemoApp.ViewModels
{
    public class AppointmentViewModel : BaseViewModel
    {
        private Appointment _selectedAppointment;

        public ObservableCollection<Appointment> Appointments { get; }
        public Command LoadAppointmentsCommand { get; }
        public Command AddAppointmentCommand { get; }
        public Command<Appointment> AppointmentTapped { get; }
        public Command<Appointment> AppointmentTapped2 { get; }

        public AppointmentViewModel()
        {
            Title = "Appointments";
            Appointments = new ObservableCollection<Appointment>();
            LoadAppointmentsCommand = new Command(async () => await ExecuteLoadAppointmentsCommand());

            AppointmentTapped = new Command<Appointment>(OnAppointmentSelected);
            AppointmentTapped2 = new Command<Appointment>(OnAppointmentSelected2);
            AddAppointmentCommand = new Command(OnAddAppointment);
        }

        async Task ExecuteLoadAppointmentsCommand()
        {
            IsBusy = true;

            try
            {
                Appointments.Clear();
                var appointments = await DataStoreAppointment.GetAppointmentsAsync(true);
                foreach (Appointment appointment in appointments)
                {
                    Appointments.Add(appointment);
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
            SelectedAppointment = null;
        }

        public Appointment SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {                
                SetProperty(ref _selectedAppointment, value);
                OnAppointmentSelected(value);
            }
        }

        private async void OnAddAppointment(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewAppointmentPage));
        }

        async void OnAppointmentSelected(Appointment appointment)
        {
            if (appointment == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(AppointmentDetailPage)}?{nameof(AppointmentDetailViewModel.AppointmentId)}={appointment.Id}");
        }
        async void OnAppointmentSelected2(Appointment appointment)
        {
            if (appointment == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(AppointmentDetailPage)}?{nameof(AppointmentDetailViewModel.AppointmentId)}={appointment.Id}");
        }

    }
}
