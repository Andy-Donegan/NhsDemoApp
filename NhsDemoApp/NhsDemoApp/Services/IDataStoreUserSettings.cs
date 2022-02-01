using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NhsDemoApp.Services
{
    public interface IDataStoreUserSettings<T>
    {
        Task<bool> UpdateUserSettingsAsync(T user);
        Task<T> GetUserSettingsAsync();
    }
}
