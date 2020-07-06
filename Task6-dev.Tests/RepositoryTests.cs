using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace Task6
{
    public class Tests
    {
        private DbContextOptions<LibraryDbContext> _options;
        
        [SetUp]
        public void Setup()
        {
            _options = UnitTestHelper.SeedData();
        }

        [Test]
        public void BookRepository_FindAll_ReturnsAllValues()
        {
            using (var context = new LibraryDbContext(_options))
            {
                var booksRepository = new Repository<Book>(context);

                var books = booksRepository.FindAll();

                Assert.AreEqual(1, books.Count());
            }
        }

        [Test]
        public async Task BookRepository_FindByCondition_ReturnsSingleValue()
        {
            using (var context = new LibraryDbContext(_options))
            {
                var booksRepository = new Repository<Book>(context);

                var book = await booksRepository.GetById(1);

                Assert.AreEqual(1, book.Id);
                Assert.AreEqual("Jon Snow", book.Author);
                Assert.AreEqual("A song of ice and fire", book.Title);
                Assert.AreEqual(1996, book.Year);
            }
        }

        [Test]
        public async Task BookRepository_AddAsync_AddsValueToDatabase()
        {
            using (var context = new LibraryDbContext(_options))
            {
                var booksRepository = new Repository<Book>(context);
                var book = new Book(){Id = 2};

                await booksRepository.AddAsync(book);
                await context.SaveChangesAsync();
                
                Assert.AreEqual(2, context.Books.Count());
            }
        }

        [Test]
        public async Task BookRepository_Delete_DeletesEntity()
        {
            using (var context = new LibraryDbContext(_options))
            {
                var bookRepository = new Repository<Book>(context);
                
                await bookRepository.DeleteById(1);
                await context.SaveChangesAsync();
                
                Assert.AreEqual(0, context.Books.Count());
            }
        }

        [Test]
        public async Task BookRepository_Update_UpdatesEntity()
        {
            using (var context = new LibraryDbContext(_options))
            {
                var booksRepository = new Repository<Book>(context);

                var book = new Book(){ Id = 1, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994};

                booksRepository.Update(book);
                await context.SaveChangesAsync();

                Assert.AreEqual(1, book.Id);
                Assert.AreEqual("John Travolta", book.Author);
                Assert.AreEqual("Pulp Fiction", book.Title);
                Assert.AreEqual(1994, book.Year);
            }
        }

        [Test]
        public void BooksController_GetBooks_ReturnsBooksModels()
        {
            //Arrange
            var mockBookService = new Mock<IBooksService>();
            mockBookService
                .Setup(x => x.GetAll())
                .Returns(GetTestBookModels());
            var bookController = new BooksController(mockBookService.Object);
            
            //Act
            var result = bookController.GetBooks();
            var values = result.Result as OkObjectResult;
            
            //Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<BookModel>>>(result);
            Assert.NotNull(values.Value);
        }

        private IEnumerable<BookModel> GetTestBookModels()
        {
            return new List<BookModel>()
            {
                new BookModel(){ Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996},
                new BookModel(){ Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994}
            };
        }

        [Test]
        public void BooksService_GetAll_ReturnsBookModels()
        {
            var expected = GetTestBookModels().ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.BookRepository.FindAll())
                .Returns(GetTestBookEntities());
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actual = bookService.GetAll().ToList();
            
            Assert.IsInstanceOf<IEnumerable<BookModel>>(actual);
            Assert.AreEqual(expected[0].Author, actual[0].Author);
            Assert.AreEqual(expected[0].Title, actual[0].Title);
            Assert.AreEqual(expected[0].Year, actual[0].Year);
            Assert.AreEqual(expected[1].Author, actual[1].Author);
            Assert.AreEqual(expected[1].Title, actual[1].Title);
            Assert.AreEqual(expected[1].Year, actual[1].Year);
        }

        private IQueryable<Book> GetTestBookEntities()
        {
            return new List<Book>()
            {
                new Book(){ Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996},
                new Book(){ Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994}
            }.AsQueryable();
        }
    }
}