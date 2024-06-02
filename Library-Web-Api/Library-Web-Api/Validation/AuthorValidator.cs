using FluentValidation;
using Library_Web_Api.Models;

namespace Library_Web_Api.Validation
{
    public class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.FirstName).NotEmpty().WithMessage("Enter author's first name.");
            RuleFor(a => a.LastName).NotEmpty().WithMessage("Enter author's last name.");
            RuleFor(a => a.YearOfBirth).NotEmpty().WithMessage("Enter author's birthday date.");
        }
    }
}
