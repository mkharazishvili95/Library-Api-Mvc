using FluentValidation;
using Library_Web_Api.Database;
using Library_Web_Api.Identity;
using Library_Web_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_Api.Service
{
    public interface IUserService
    {
        Task<User> RegisterNewUser(User newUser);
        Task<User> UserLoginForm(UserLoginForm login);
    }

    public class UserService : IUserService
    {
        private readonly LibraryContext _context;
        private readonly IValidator<User> _userValidator;

        public UserService(LibraryContext context, IValidator<User> userValidator)
        {
            _context = context;
            _userValidator = userValidator;
        }

        public async Task<User> RegisterNewUser(User newUser)
        {
            var validatorResults = _userValidator.Validate(newUser);
            if (!validatorResults.IsValid)
            {
                return null;
            }

            var registerNewUser = new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.UserName,
                Password = PasswordHashing.HashPassword(newUser.Password),
                Role = Roles.User
            };

            await _context.AddAsync(registerNewUser);
            await _context.SaveChangesAsync();
            return registerNewUser;
        }

        public async Task<User> UserLoginForm(UserLoginForm login)
        {
            var existingUser = await _context.Users
                .SingleOrDefaultAsync(user => user.UserName == login.UserName);

            if (existingUser == null || PasswordHashing.HashPassword(login.Password) != existingUser.Password || login.UserName.ToUpper() != existingUser.UserName.ToUpper())
            {
                return null;
            }

            return existingUser;
        }
    }
}
