using FluentValidation;
using Library_Web_Api.Database;
using Library_Web_Api.Models;

namespace Library_Web_Api.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly LibraryContext _context;
        public UserValidator(LibraryContext context)
        {
            _context = context;
            RuleFor(user => user.FirstName).NotEmpty().WithMessage("Enter your FirstName.");
            RuleFor(user => user.LastName).NotEmpty().WithMessage("Enter your Lastname.");
            RuleFor(user => user.UserName).NotEmpty().WithMessage("Enter your Username.")
            .Length(6, 15).WithMessage("Username length should be from 6 to 15 chars or numbers.")
            .Must(DifferentUserName).WithMessage("Username already exists. Try another.");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Enter your password")
                .Length(6, 15).WithMessage("Password length should be between 6 and 15 chars or numbers!");
        }
        private bool DifferentUserName(string userName)
        {
            var theSameUserName = _context.Users.SingleOrDefault(user => user.UserName.ToUpper() == userName.ToUpper());
            return theSameUserName == null;
        }
    }
}
