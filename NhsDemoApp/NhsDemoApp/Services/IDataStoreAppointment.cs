using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NhsDemoApp.Services
{
    public interface IDataStoreAppointment<T>
    {
        Task<bool> AddAppointmentAsync(T appointment);
        Task<bool> UpdateAppointmentAsync(T appointment);
        Task<bool> DeleteAppointmentAsync(string id);
        Task<T> GetAppointmentAsync(string id);
        Task<IEnumerable<T>> GetAppointmentsAsync(bool forceRefresh = false);

    }
}
