using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace NhsDemoApp.ViewModels
{
    [QueryProperty(nameof(AppointmentId), nameof(AppointmentId))]
    public class AppointmentDetailViewModel2 : BaseViewModel
    {

        private string appointmentId;
        private TimeSpan? arrivalTime;
        private TimeSpan? departureTime;
        private DateTime dueTime;
        private string contact;
        private bool isCompleted;
        public string Id { get;set; }

        public Command<Appointment> AppointmentTapped { get; }

        public TimeSpan? ArrivalTime
        {
            get => arrivalTime;
            set => SetProperty(ref arrivalTime, value);
        }

        public TimeSpan? DepartureTime
        {
            get => departureTime;
            set => SetProperty(ref departureTime, value);
        }

        public AppointmentDetailViewModel2()
        {
            Title = "Appointment Details";
            AppointmentTapped = new Command<Appointment>(OnArrivalTimePickerPropertyChanged);
        }

        public DateTime DueTime
        {
            get => dueTime;
            set => SetProperty(ref dueTime, value);
        }

        public string Contact
        {
            get => contact;
            set => SetProperty(ref contact, value);
        }

        public bool IsCompleted
        {
            get => isCompleted;
            set => SetProperty(ref isCompleted, value);
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
                LoadAppointmentId(value);
            }
        }

        public async void LoadAppointmentId(string appointmentId)
        {
            try
            {
                var appointment = await DataStoreAppointment.GetAppointmentAsync(appointmentId);
                Id = appointment.Id;
                DueTime = appointment.DueTime;
                Contact = appointment.Contact;
                IsCompleted = appointment.IsCompleted;
                try
                {
                    ArrivalTime = appointment.ArrivalTime;
                }
                catch (Exception ex)
                {
                    ArrivalTime = appointment.DueTime.TimeOfDay;
                }
                try
                {
                    DepartureTime = appointment.DepartureTime;
                }
                catch (Exception ex)
                {
                    DepartureTime = appointment.DueTime.TimeOfDay;
                    DepartureTime += TimeSpan.FromHours(1);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Load Appointment");
            }
        }

        public async void OnArrivalTimePickerPropertyChanged(Appointment appointment)
        {

                Console.WriteLine("fdfdfdffdfsdfsdfsdfsdfsdf");

        }
    }
}
