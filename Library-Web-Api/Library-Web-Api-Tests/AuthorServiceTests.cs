using Library_Web_Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeServices
{
    [TestFixture]
    public class AuthorServiceTests
    {
        private IAuthorService _authorService;

        [SetUp]
        public void Setup()
        {
            _authorService = new FakeAuthorService();
        }

        [Test]
        public async Task GetAllAuthors_ReturnsAllAuthors()
        {
            var authors = await _authorService.GetAllAuthors();
            Assert.IsNotNull(authors);
            Assert.AreEqual(3, authors.Count());
        }

        [Test]
        public async Task GetAuthorById_ReturnsCorrectAuthor()
        {
            var author = await _authorService.GetAuthorById(1);
            Assert.IsNotNull(author);
            Assert.AreEqual("George", author.FirstName);
            Assert.AreEqual("Orwell", author.LastName);
        }

        [Test]
        public async Task GetAuthorByName_ReturnsCorrectAuthor()
        {
            var author = await _authorService.GetAuthorByName("Jane");
            Assert.IsNotNull(author);
            Assert.AreEqual("Jane", author.FirstName);
            Assert.AreEqual("Austen", author.LastName);
        }
    }
}
