using Library_Web_Api.Models;
using Library_Web_Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeServices
{
    [TestFixture]
    public class BookServiceTests
    {
        private IBookService _bookService;

        [SetUp]
        public void Setup()
        {
            _bookService = new FakeBookService();
        }

        [Test]
        public async Task GetAllBooks_ReturnsAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            Assert.IsNotNull(books);
            Assert.AreEqual(3, books.Count());
        }

        [Test]
        public async Task GetBookByBookName_ReturnsCorrectBook()
        {
            var book = await _bookService.GetBookByBookName("1984");
            Assert.IsNotNull(book);
            Assert.AreEqual("1984", book.Title);
        }

        [Test]
        public async Task GetAllBooksIncludingAuthors_ReturnsAllBooksWithAuthors()
        {
            var books = await _bookService.GetAllBooksIncludingAuthors();
            Assert.IsNotNull(books);
            Assert.AreEqual(3, books.Count());
            Assert.IsTrue(books.All(b => b.Authors != null && b.Authors.Any()));
        }

        [Test]
        public async Task GetBookById_ReturnsCorrectBook()
        {
            var book = await _bookService.GetBookById(1);
            Assert.IsNotNull(book);
            Assert.AreEqual("1984", book.Title);
        }

        [Test]
        public async Task GetBookByAuthorsName_ReturnsCorrectBook()
        {
            var book = await _bookService.GetBookByAuthorsName("George");
            Assert.IsNotNull(book);
            Assert.AreEqual("1984", book.Title);
        }

        [Test]
        public async Task AddBook_AddsBookSuccessfully()
        {
            var newBook = new Book { Title = "New Book",Authors = new List<Author> { new Author { FirstName = "New", LastName = "Author" } } };
            var addedBook = await _bookService.AddBook(newBook);
            Assert.IsNotNull(addedBook);
            Assert.AreEqual("New Book", addedBook.Title);
        }
        [Test]
        public async Task GetAvailableBooks_ReturnsNotNull_WhenAvailableBooksExist()
        {
            var availableBooks = await _bookService.GetAvailableBooks();
            Assert.IsNotNull(availableBooks);
        }

        [Test]
        public async Task GetBookByIdIncludingAuthors_ReturnsCorrectBookWithAuthors()
        {
            var bookId = 1;

            var book = await _bookService.GetBookByIdIncludingAuthors(bookId);

            Assert.IsNotNull(book);
            Assert.AreEqual(bookId, book.Id);
            Assert.IsTrue(book.Authors != null && book.Authors.Any());
        }

        [Test]
        public async Task DeleteBook_DeletesBookSuccessfully()
        {
            var result = await _bookService.DeleteBook(1);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task TakingOutTheBook_UpdatesBookStatusSuccessfully()
        {
            var result = await _bookService.TakingOutTheBook(1);
            Assert.IsTrue(result);

            var book = await _bookService.GetBookById(1);
            Assert.IsTrue(book.IsCheckedOut);
        }

        [Test]
        public async Task ReturnTheBook_UpdatesBookStatusSuccessfully()
        {
            await _bookService.TakingOutTheBook(1);

            var result = await _bookService.ReturnTheBook(1);
            Assert.IsTrue(result);

            var book = await _bookService.GetBookById(1);
            Assert.IsFalse(book.IsCheckedOut);
        }

        [Test]
        public async Task UpdateBook_BookExists_ReturnsTrue()
        {
            var bookId = 1;
            var updatedBook = new Book
            {
                Id = bookId,
                Title = "Updated Book 1",
                Description = "Updated Description 1",
                Image = "UpdatedImage1.png",
                PublicationDate = new DateTime(2005, 1, 1),
                Authors = new List<Author> { new Author { Id = 1, FirstName = "Updated John", LastName = "Doe", YearOfBirth = DateTime.Parse("1993/03/03") } }
            };
            var result = await _bookService.UpdateBook(bookId, updatedBook);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateBook_BookDoesNotExist_ReturnsFalse()
        {
            var bookId = 999;
            var updatedBook = new Book
            {
                Id = bookId,
                Title = "Non-existent Book",
                Description = "Non-existent Description",
                Image = "NonExistentImage.png",
                PublicationDate = new DateTime(2010, 1, 1),
                Authors = new List<Author> { new Author { Id = 999, FirstName = "Non-existent Author", LastName = "Gela", YearOfBirth = DateTime.Parse("1990/07/21") } }
            };
            var result = await _bookService.UpdateBook(bookId, updatedBook);
            Assert.IsFalse(result);
        }
    }
}
