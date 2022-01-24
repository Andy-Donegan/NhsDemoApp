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
            var dueTime = new DateTime(2022, 3, 15, 9, 00, 00);
            string[] contactList = { "John Smith", "Charlie Donegan", "Sarah Sullivan", "Roger Mellee", "William Tyson", "Willow Teale", "Louise McCrone" };

            appointments = new List<Appointment>()
            {

            };
                for (int i = 0; i < contactList.Length; i++)
            {
                new Appointment { Id = Guid.NewGuid().ToString(), DueTime = dueTime.AddHours(i), Contact = contactList[i], ArrivalTime = DateTime.Now, IsCompleted = false, User = "", Organisation = "" };
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