using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NhsDemoApp.Services
{
    public interface IDataStoreUser<T>
    {
        Task<bool> UpdateUserAsync(T user);
        Task<T> GetUserAsync();
    }
}
