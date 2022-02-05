using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NhsDemoApp.Services
{
    public class MockDataStoreUserSettings : IDataStoreUserSettings<UserSettings>
    {
        public UserSettings _userSettings;

        public MockDataStoreUserSettings()
        {
            _userSettings = new UserSettings { FirstName = "Willow", LastName = "Teale", Organisation = "BDCT", CurrentTime=DateTime.Now.TimeOfDay , SecurityPin = 1234 };

        }

        public async Task<bool> UpdateUserSettingsAsync(UserSettings userSettings)
        {
            _userSettings = userSettings;

            return await Task.FromResult(true);
        }

        public async Task<UserSettings> GetUserSettingsAsync()
        {
            return await Task.FromResult(_userSettings);
        }
    }
}