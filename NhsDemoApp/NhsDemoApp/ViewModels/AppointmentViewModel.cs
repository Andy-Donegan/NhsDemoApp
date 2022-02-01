﻿using NhsDemoApp.Models;
using NhsDemoApp.Services;
using NhsDemoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NhsDemoApp.ViewModels
{
    public class AppointmentViewModel : BaseViewModel
    {
        private Appointment _selectedAppointment;
        public Command ExportToExcelCommand { private set; get; }
        private ExcelService excelService;
        public ObservableCollection<Appointment> Appointments { get; }
        public Command LoadAppointmentsCommand { get; }
        public Command<Appointment> AppointmentTapped { get; }
        public Command LoadMap { get; }

        public AppointmentViewModel()
        {
            Title = "Appointments";
            Appointments = new ObservableCollection<Appointment>();
            LoadAppointmentsCommand = new Command(async () => await ExecuteLoadAppointmentsCommand());

            AppointmentTapped = new Command<Appointment>(OnAppointmentSelected);

            ExportToExcelCommand = new Command(async () => await ExportToExcel());
            excelService = new ExcelService();

            LoadMap = new Command<Appointment>(OnLoadMap);
        }

        private async void OnLoadMap(Appointment appointment)
        {
            await Shell.Current.GoToAsync($"{nameof(MapPage)}?{nameof(MapPageViewModel.AppointmentId)}={appointment.Id}");
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
                    if (appointment.ArrivalTime != null && appointment.DepartureTime != null)
                    {
                        appointment.IsCompleted = true;
                        appointment.TimesRequired = false;
                    }
                    else
                    {
                        appointment.TimesRequired = true;
                    }
                    if(appointment.DueTime >= DateTime.Now && appointment.IsCompleted != true)
                    {
                        appointment.IsLate = true;
                    }
                    else
                    {
                        appointment.IsLate = false;
                    }
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

        async void OnAppointmentSelected(Appointment appointment)
        {
            if (appointment == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(AppointmentDetailPage)}?{nameof(AppointmentDetailViewModel.AppointmentId)}={appointment.Id}");
        }
        async Task ExportToExcel()
        {
            var fileName = $"Demo-{Guid.NewGuid()}.xlsx";
            string filepath = excelService.GenerateExcel(fileName);

            var data = new ExcelModel
            {
                Headers = new List<string>() { "Date", "DueTime", "Contact", "Arrival" , "Departure", "Complete", "User" , "Organisation", "Latitude", "Longitude", "GoogleMap Link" }
            };

            foreach (var appointment in Appointments)
            {
                var arrivalTime = "No time logged";
                var departureTime = "";
                try
                {
                    arrivalTime = new DateTime(appointment.ArrivalTime.Value.Ticks).ToString("HH:mm");
                }
                catch (Exception ex)
                {

                }
                try
                {
                    departureTime = new DateTime(appointment.DepartureTime.Value.Ticks).ToString("HH:mm");
                }
                catch (Exception ex)
                {

                }

                var googleString = "https://www.google.co.uk/maps/@" + appointment.Latitude.ToString() + "," + appointment.Longitude.ToString() + ",383m/data!3m1!1e3";

                data.Values.Add(new List<string>() { appointment.DueTime.ToShortDateString(), appointment.DueTime.ToShortTimeString(), appointment.Contact, arrivalTime, departureTime, appointment.IsCompleted.ToString(), appointment.User, appointment.Organisation, appointment.Latitude.ToString(), appointment.Longitude.ToString(), googleString }) ;
            }

            excelService.InsertDataIntoSheet(filepath, "Demo", data);

            await Launcher.OpenAsync(new OpenFileRequest()
            {
                File = new ReadOnlyFile(filepath)
            });
        }
    }
}
