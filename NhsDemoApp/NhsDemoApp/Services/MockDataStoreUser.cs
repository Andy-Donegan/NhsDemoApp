using NhsDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NhsDemoApp.Services
{
    public class MockDataStoreUser : IDataStoreUser<User>
    {
        public User _user;

        public MockDataStoreUser()
        {
            _user = new User { FirstName = "Cara", LastName = "Smithson", Organisation = "BDCT" };

        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _user = user;

            return await Task.FromResult(true);
        }

        public async Task<User> GetUserAsync()
        {
            return await Task.FromResult(_user);
        }
    }
}