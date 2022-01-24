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
        public string appointmentId;

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
                //fill in other fields.

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Load Appointment");
            }
        }
    }
}
