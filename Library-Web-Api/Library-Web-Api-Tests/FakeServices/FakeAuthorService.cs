using Library_Web_Api.Models;
using Library_Web_Api.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeServices
{
    public class FakeAuthorService : IAuthorService
    {
        private readonly List<Author> _authors;

        public FakeAuthorService()
        {
            _authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "George", LastName = "Orwell" },
                new Author { Id = 2, FirstName = "Jane", LastName = "Austen" },
                new Author { Id = 3, FirstName = "Mark", LastName = "Twain" }
            };
        }

        public Task<IEnumerable<Author>> GetAllAuthors()
        {
            return Task.FromResult(_authors.AsEnumerable());
        }

        public Task<Author> GetAuthorById(int authorId)
        {
            var author = _authors.SingleOrDefault(a => a.Id == authorId);
            return Task.FromResult(author);
        }

        public Task<Author> GetAuthorByName(string authorName)
        {
            var author = _authors
                .SingleOrDefault(a => a.FirstName.ToUpper() == authorName.ToUpper() || a.LastName.ToUpper() == authorName.ToUpper());
            return Task.FromResult(author);
        }
    }
}
