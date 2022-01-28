using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
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
        private Appointment _selectedAppointment;
        public Appointment Appointment { get; set; }
        public Command LoadAppointmentCommand { get; }
        public Command<Appointment> AppointmentTapped { get; }
        public Command<Appointment> AppointmentTapped2 { get; }

        public AppointmentDetailViewModel()
        {
            Title = "Appointment Details";
            Appointment = new Appointment();
            LoadAppointmentCommand = new Command(async () => await ExecuteLoadAppointmentCommand());

            //AppointmentTapped = new Command<Appointment>(OnAppointmentSelected);
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
                //Task task = ExecuteLoadAppointmentCommand();
            }
        }

        async Task ExecuteLoadAppointmentCommand()
        {
            IsBusy = true;

            try
            {
                Appointment = await DataStoreAppointment.GetAppointmentAsync(appointmentId);
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
            //SelectedAppointment = null;
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
