using Library_Web_Api.Database;
using Library_Web_Api.Models;
using Library_Web_Api.Validation;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_Api.Service
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<Author> GetAuthorById(int authorId);
        Task<Author> GetAuthorByName(string  authorName);
    }
    public class AuthorService : IAuthorService
    {
        private readonly LibraryContext _context;
        public AuthorService(LibraryContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorById(int authorId)
        {
            return await _context.Authors.SingleOrDefaultAsync(author => author.Id == authorId); 
        }

        public async Task<Author> GetAuthorByName(string authorName)
        {
            var existingAuthor = await _context.Authors
                .SingleOrDefaultAsync(author => author.FirstName.ToUpper() == authorName.ToUpper() || 
                author.LastName.ToUpper() == authorName.ToUpper());
            return existingAuthor;
        }
    }
}
