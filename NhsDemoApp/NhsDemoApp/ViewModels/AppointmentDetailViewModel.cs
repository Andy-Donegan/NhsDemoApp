using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace NhsDemoApp.ViewModels
{
    [QueryProperty(nameof(AppointmentId), nameof(AppointmentId))]
    public class AppointmentDetailViewModel : BaseViewModel
    {

        private string appointmentId;
        private DateTime dueTime;
        private string contact;
        private bool isCompleted;

        public string Id { get;set; }

        public AppointmentDetailViewModel()
        {
            Title = "Appointment Details";
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Load Appointment");
            }
        }
    }
}
