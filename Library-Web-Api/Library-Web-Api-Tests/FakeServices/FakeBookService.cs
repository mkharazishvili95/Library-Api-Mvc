using Library_Web_Api.Models;
using Library_Web_Api.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeServices
{
    public class FakeBookService : IBookService
    {
        private readonly List<Book> _books;
        private readonly List<Author> _authors;

        public FakeBookService()
        {
            _authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "George", LastName = "Orwell" },
                new Author { Id = 2, FirstName = "Jane", LastName = "Austen" },
                new Author { Id = 3, FirstName = "Mark", LastName = "Twain" }
            };

            _books = new List<Book>
            {
                new Book { Id = 1, Title = "1984", Authors = new List<Author> { _authors[0] }, IsCheckedOut = false },
                new Book { Id = 2, Title = "Pride and Prejudice", Authors = new List<Author> { _authors[1] }, IsCheckedOut = false },
                new Book { Id = 3, Title = "Adventures of Huckleberry Finn", Authors = new List<Author> { _authors[2] }, IsCheckedOut = false }
            };
        }

        public Task<Book> AddBook(Book newBook)
        {
            newBook.Id = _books.Count + 1;
            _books.Add(newBook);
            return Task.FromResult(newBook);
        }

        public Task<bool> DeleteBook(int bookId)
        {
            var book = _books.SingleOrDefault(b => b.Id == bookId);
            if (book == null)
                return Task.FromResult(false);

            _books.Remove(book);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Book>> GetAllBooks()
        {
            return Task.FromResult(_books.AsEnumerable());
        }

        public Task<IEnumerable<Book>> GetAllBooksIncludingAuthors()
        {
            return Task.FromResult(_books.AsEnumerable());
        }

        public async Task<IEnumerable<Book>> GetAvailableBooks()
        {
            return _books.Where(book => !book.IsCheckedOut).ToList();
        }

        public Task<Book> GetBookByAuthorsName(string authorsName)
        {
            var book = _books.FirstOrDefault(b => b.Authors.Any(a => a.FirstName.ToUpper() == authorsName.ToUpper() || a.LastName.ToUpper() == authorsName.ToUpper()));
            return Task.FromResult(book);
        }

        public Task<Book> GetBookByBookName(string bookName)
        {
            var book = _books.FirstOrDefault(b => b.Title.ToUpper().Contains(bookName.ToUpper()));
            return Task.FromResult(book);
        }

        public Task<Book> GetBookById(int bookId)
        {
            var book = _books.SingleOrDefault(b => b.Id == bookId);
            return Task.FromResult(book);
        }

        public Task<Book> GetBookByIdIncludingAuthors(int bookId)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            return Task.FromResult(book);
        }

        public Task<bool> ReturnTheBook(int bookId)
        {
            var book = _books.SingleOrDefault(b => b.Id == bookId);
            if (book == null || !book.IsCheckedOut)
                return Task.FromResult(false);

            book.IsCheckedOut = false;
            return Task.FromResult(true);
        }

        public Task<bool> TakingOutTheBook(int bookId)
        {
            var book = _books.SingleOrDefault(b => b.Id == bookId);
            if (book == null || book.IsCheckedOut)
                return Task.FromResult(false);

            book.IsCheckedOut = true;
            return Task.FromResult(true);
        }

        public Task<bool> UpdateBook(int bookId, [FromBody] Book updatedBookModel)
        {
            var existingBook = _books.SingleOrDefault(b => b.Id == bookId);
            if (existingBook == null)
            {
                return Task.FromResult(false);
            }
            existingBook.Title = updatedBookModel.Title;
            existingBook.Description = updatedBookModel.Description;
            existingBook.Image = updatedBookModel.Image;
            existingBook.PublicationDate = updatedBookModel.PublicationDate;
            existingBook.Authors.Clear();
            foreach (var author in updatedBookModel.Authors)
            {
                existingBook.Authors.Add(author);
            }
            return Task.FromResult(true);
        }
    }
}
