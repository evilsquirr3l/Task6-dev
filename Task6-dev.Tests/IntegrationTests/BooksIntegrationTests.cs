using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Models;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using WebApi;

namespace Task6.IntegrationTests
{
    [TestFixture]
    public class BooksIntegrationTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory _factory;
        private const string RequestUri = "api/books/";
        
        [OneTimeSetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }
        
        [Test]
        public async Task BooksController_GetByFilter_ReturnsAllWithNullFilter()
        {
            var httpResponse = await _client.GetAsync(RequestUri);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(stringResponse);
            
            Assert.AreEqual(2, books.Count());
        }
        
        [Test]
        public async Task BooksController_GetByFilter_ReturnsBooksThatApplyFilter()
        {
            var httpResponse = await _client.GetAsync($"{RequestUri}?Author=Jon%20Snow&Year=1996");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(stringResponse);

            foreach (var book in books)
            {
                Assert.AreEqual("Jon Snow", book.Author);
                Assert.AreEqual(1996, book.Year);
            }
        }

        [Test]
        public async Task BooksController_Add_AddsBookToDatabase()
        {
            var book = new BookModel{Author = "Charles Dickens", Title = "A Tale of Two Cities", Year = 1859};
            var bookId = 3;
            var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(RequestUri, content);

            httpResponse.EnsureSuccessStatusCode();

            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();
                var databaseBook = await context.Books.FindAsync(bookId);
                Assert.AreEqual(bookId, databaseBook.Id);
                Assert.AreEqual(book.Author, databaseBook.Author);
                Assert.AreEqual(book.Title, databaseBook.Title);
                Assert.AreEqual(3, context.Books.Count());
            }
        }

        [Test]
        public async Task BooksController_Update_UpdatesBookInDatabase()
        {
            var book = new BookModel{Id = 2, Author = "Honore de Balzac", Title = "Lost Illusions", Year = 1843};
            var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(RequestUri, content);

            httpResponse.EnsureSuccessStatusCode();
            
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();
                var databaseBook = await context.Books.FindAsync(book.Id);
                Assert.AreEqual(book.Id, databaseBook.Id);
                Assert.AreEqual(book.Author, databaseBook.Author);
                Assert.AreEqual(book.Title, databaseBook.Title);
                Assert.AreEqual(2, context.Books.Count());
            }
        }

        [Test]
        public async Task BooksController_DeleteById_DeletesBookFromDatabase()
        {
            var bookId = 1;
            var httpResponse = await _client.DeleteAsync(RequestUri + bookId);

            httpResponse.EnsureSuccessStatusCode();
            
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();
                
                Assert.AreEqual(2, context.Books.Count());
            }
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}