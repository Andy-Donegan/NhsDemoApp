using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NhsDemoApp.Services
{
    public class MockDataStoreAppointment : IDataStoreAppointment<Appointment>
    {
        readonly List<Appointment> appointments;
        
        public MockDataStoreAppointment()
        {
            var timeNow = DateTime.Now;
            var dueTime = DateTime.Now;
            var timeSpan = new TimeSpan(9, 0, 0);
            dueTime = dueTime.Date + timeSpan;
            bool isLate;
            string[] contactList = { "John Smith", "Charlie Donegan", "Sure Start Shipley", "Sarah Sullivan", "Roger Mellee", "New Mill" , "William Tyson", "Willow Teale", "Jacobs Well", "Louise McCrone" };
            var random = new Random();

            appointments = new List<Appointment>();
            for (int i = 0; i < contactList.Length; i++)
            {
                var timeCheck = dueTime.AddHours(i);
                //var dueTime = new DateTime(2022, 3, 15, 9, 00, 00);

                if (timeNow >= timeCheck)
                {
                    isLate = true;
                }
                else
                {
                    isLate = false;
                }
                var randomBool = random.Next(2) == 1;
                appointments.Add(new Appointment { Id = Guid.NewGuid().ToString(), DueTime = timeCheck, Contact = contactList[i], ArrivalTime = null, DepartureTime= null, IsCompleted = randomBool, IsLate = isLate, OnSite = false , User = "", Organisation = "" });
            };
        }

        public async Task<bool> AddAppointmentAsync(Appointment appointment)
        {
            appointments.Add(appointment);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            var oldAppointment = appointments.Where((Appointment arg) => arg.Id == appointment.Id).FirstOrDefault();
            appointments.Remove(oldAppointment);
            appointments.Add(appointment);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAppointmentAsync(string id)
        {
            var oldAppointment = appointments.Where((Appointment arg) => arg.Id == id).FirstOrDefault();
            appointments.Remove(oldAppointment);

            return await Task.FromResult(true);
        }

        public async Task<Appointment> GetAppointmentAsync(string id)
        {
            return await Task.FromResult(appointments.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(appointments);
        }
    }
}