using NhsDemoApp.Models;
using NhsDemoApp.Services;
using NhsDemoApp.Views;
using Plugin.LocalNotification;
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
        public Command RoastWillow { get; }
        public UserSettings UserSettings { get; set; }

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

        async Task LoadUserSettings()
        {
            try
            {
                UserSettings = await DataStoreUserSettings.GetUserSettingsAsync();
            }
            catch (Exception ex)
            {
                UserSettings = new UserSettings
                {
                    CurrentTime = DateTime.Now.TimeOfDay,
                    FirstName = "Default",
                    LastName = "Name",
                    Organisation = "NHS"
                };
            }
        }

        async Task ExecuteLoadAppointmentsCommand()
        {
            IsBusy = true;
            await LoadUserSettings();

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
                    if(appointment.DueTime.TimeOfDay < UserSettings.CurrentTime && appointment.IsCompleted != true)
                    {
                        appointment.IsLate = true;
                    }
                    else
                    {
                        appointment.IsLate = false;
                    }
                        appointment.User = UserSettings.FirstName + " " + UserSettings.LastName;
                        appointment.Organisation = UserSettings.Organisation;

                    if (UserSettings.OnSiteID == appointment.Id)
                    {
                        appointment.OnSite = true;
                    }
                    else
                    {
                        appointment.OnSite= false;
                    }

                    Appointments.Add(appointment);
                }
                //TODO Add test notifications Call here or in the foreach above. use SendLocalNotification to send message.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task SendLocalNotification()
        {
            //TODO sort timers etc for Notifications.
            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "Title",
                Subtitle = "Subtitle",
                Description = "Description",
                BadgeNumber = 5,
                ReturningData = "100",
                Schedule =
                {
                    NotifyTime = DateTime.Now.AddSeconds(10) // Used for Scheduling local notification, if not specified notification will show immediately.
                },
                Android =
                {
                    IconSmallName =
                    {
                        ResourceName = "icon_home",
                    },
                    Color =
                    {
                        ResourceName = "colorPrimary"
                    }
                }
            };
            await NotificationCenter.Current.Show(notification);
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
                var departureTime = "No time logged";
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

                data.Values.Add(new List<string>() { appointment.DueTime.ToShortDateString(), appointment.DueTime.ToShortTimeString(), appointment.Contact, arrivalTime, departureTime, appointment.IsCompleted.ToYesNoString(), appointment.User, appointment.Organisation, appointment.Latitude.ToString(), appointment.Longitude.ToString(), googleString }) ;
            }

            excelService.InsertDataIntoSheet(filepath, "Demo", data);

            await Launcher.OpenAsync(new OpenFileRequest()
            {
                File = new ReadOnlyFile(filepath)
            });
        }
    }
}
