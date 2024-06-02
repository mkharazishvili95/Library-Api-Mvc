using FakeServices;
using Library_Web_Api.Identity;
using Library_Web_Api.Models;
using Library_Web_Api.Service;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FakeServices
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new FakeUserService();
        }

        [Test]
        public async Task RegisterNewUser_SuccessfulRegistration()
        {
            var userRegisterModel = new User
            {
                FirstName = "Misho",
                LastName = "Kharazishvili",
                UserName = "Misho123",
                Password = PasswordHashing.HashPassword("Misho123"),
            };
            var registeredUser = await _userService.RegisterNewUser(userRegisterModel);
            Assert.IsNotNull(registeredUser);
            Assert.AreEqual(userRegisterModel.FirstName, registeredUser.FirstName);
            Assert.AreEqual(userRegisterModel.LastName, registeredUser.LastName);
            Assert.AreEqual(userRegisterModel.UserName, registeredUser.UserName);
            Assert.AreEqual(PasswordHashing.HashPassword(userRegisterModel.Password), registeredUser.Password);
        }

        [Test]
        public async Task UserLoginForm_SuccessfulLogin()
        {
            var userLoginModel = new UserLoginForm
            {
                UserName = "Misho123",
                Password = "Misho123"
            };
            var loggedInUser = await _userService.UserLoginForm(userLoginModel);
            Assert.IsNotNull(loggedInUser);
            Assert.AreEqual("Misho", loggedInUser.FirstName);
            Assert.AreEqual("Shalva123", loggedInUser.UserName); 
        }
    }
}
