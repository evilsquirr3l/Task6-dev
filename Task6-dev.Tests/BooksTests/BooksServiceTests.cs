using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Business.Services;
using Data;
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
                .Returns(GetTestBookEntities().AsQueryable);
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actual = bookService.GetAll().ToList();

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
                Assert.AreEqual(expected[i].Author, actual[i].Author);
                Assert.AreEqual(expected[i].Title, actual[i].Title);
            }
        }
        
        private IEnumerable<BookModel> GetTestBookModels()
        {
            return new List<BookModel>()
            {
                new BookModel {Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909},
                new BookModel {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994},
                new BookModel {Id = 3, Author = "Jack London", Title = "The Call of the Wild", Year = 1903},
                new BookModel {Id = 4, Author = "Robert Jordan", Title = "Lord of Chaos", Year = 1994}
            };
        }

        [Test]
        public async Task BooksService_GetByIdAsync_ReturnsBookModel()
        {
            var expected = GetTestBookModels().First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.BookRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestBookEntities().First);
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actual = await bookService.GetByIdAsync(1);
            
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Author, actual.Author);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        private List<Book> GetTestBookEntities()
        {
            return new List<Book>()
            {
                new Book {Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909},
                new Book {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994},
                new Book {Id = 3, Author = "Jack London", Title = "The Call of the Wild", Year = 1903},
                new Book {Id = 4, Author = "Robert Jordan", Title = "Lord of Chaos", Year = 1994}
            };
        }
        
        [Test]
        public async Task BooksService_AddAsync_AddsModel()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var book = new BookModel {Id = 100, Author = "@squirr3l"};
            
            //Act
            await bookService.AddAsync(book);
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.AddAsync(It.Is<Book>(b => b.Author == book.Author && b.Id == book.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public async Task BooksService_DeleteByIdAsync_DeletesBook(int bookId)
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.DeleteByIdAsync(It.IsAny<int>()));
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            //Act
            await bookService.DeleteByIdAsync(bookId);
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.DeleteByIdAsync(bookId), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }
        
        [Test]
        public async Task BooksService_UpdateAsync_UpdatesBook()
        {
            //Arrange
            var book = new BookModel{Id = 1, Author = "Jack London"};
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            //Act
            await bookService.UpdateAsync(book);
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.Update(It.Is<Book>(b => b.Author == book.Author && b.Id == book.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void BooksService_GetByFilter_ReturnsBooksByAuthor()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.FindAll()).Returns(GetTestBookEntities().AsQueryable);
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var filter = new FilterSearchModel{Author = "Jack London"};
            
            //Act
            var filteredBooks = bookService.GetByFilter(filter).ToList();
            
            Assert.AreEqual(2, filteredBooks.Count);
            foreach (var book in filteredBooks)
            {
                Assert.AreEqual(filter.Author, book.Author);
            }
        }
        
        [Test]
        public void BooksService_GetByFilter_ReturnsBooksByYear()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.FindAll()).Returns(GetTestBookEntities().AsQueryable);
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var filter = new FilterSearchModel{Year = 1994};
            
            var filteredBooks = bookService.GetByFilter(filter).ToList();
            
            Assert.AreEqual(2, filteredBooks.Count);
            foreach (var book in filteredBooks)
            {
                Assert.AreEqual(filter.Year, book.Year);
            }
        }
    }
}