using NhsDemoApp.Models;
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
        public Command<Appointment> AppointmentTapped { get; }
        public Command<Appointment> AppointmentTapped2 { get; }

        public AppointmentDetailViewModel()
        {
            Title = "Appointment Details";
            Appointments = new ObservableCollection<Appointment>();
            LoadAppointmentCommand = new Command(async () => await ExecuteLoadAppointmentCommand());
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

        //public Appointment SelectedAppointment
        //{
        //    get => _selectedAppointment;
        //    set
        //    {
        //        SetProperty(ref _selectedAppointment, value);
        //        OnAppointmentSelected(value);
        //    }
        //}

        //async void OnAppointmentSelected(Appointment appointment)
        //{
        //    if (appointment == null)
        //        return;
        //    await Shell.Current.GoToAsync($"{nameof(AppointmentDetailPage)}?{nameof(AppointmentDetailViewModel.AppointmentId)}={appointment.Id}");
        //}
    }
}
