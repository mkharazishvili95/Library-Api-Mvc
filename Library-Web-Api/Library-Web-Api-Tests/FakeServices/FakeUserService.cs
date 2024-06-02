using Library_Web_Api.Identity;
using Library_Web_Api.Models;
using Library_Web_Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeServices
{
    public class FakeUserService : IUserService
    {
        public Task<User> RegisterNewUser(User newUser)
        {
            var fakeUser = new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.UserName,
                Password = PasswordHashing.HashPassword(newUser.Password),
                Role = Roles.User
            };

            return Task.FromResult(fakeUser);
        }

        public Task<User> UserLoginForm(UserLoginForm login)
        {
            var fakeUser = new User
            {
                FirstName = "Misho",
                LastName = "Kharazishvili",
                UserName = "Shalva123", 
                Password = PasswordHashing.HashPassword("Misho123"),
                Role = Roles.User
            };
            return Task.FromResult(fakeUser);
        }
    }
}
