using System.Collections.Generic;
using System.Linq;
using Business.Models;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace Task6.BooksTests
{
    public class BooksServiceTests
    {
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

        private IEnumerable<BookModel> GetTestBookModels()
        {
            return new List<BookModel>()
            {
                new BookModel(){ Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996},
                new BookModel(){ Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994}
            };
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