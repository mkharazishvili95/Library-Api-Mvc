using FluentValidation;
using Library_Web_Api.Models;

namespace Library_Web_Api.Validation
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Title).NotEmpty().WithMessage("Enter title.");
            RuleFor(book => book.Description).NotEmpty().WithMessage("Enter description.");
            RuleFor(book => book.PublicationDate).NotEmpty().WithMessage("Enter publication date.");
            RuleForEach(book => book.Authors).SetValidator(new AuthorValidator());
        }
    }
}
