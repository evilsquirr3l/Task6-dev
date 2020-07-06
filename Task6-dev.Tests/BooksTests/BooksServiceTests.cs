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
        //TODO: get by filter, is book returned
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

        [Test]
        public async Task BooksService_GetByIdAsync_ReturnsBookModel()
        {
            var expected = GetTestBookModels().First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.BookRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
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
                new Book() {Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996},
                new Book() {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994}
            };
        }

        [Test]
        public async Task BooksService_AddAsync_AddsModel()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.AddAsync(It.IsAny<Book>()));
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            //Act
            await bookService.AddAsync(new BookModel(){Id = 100, Author = "@squirr3l"});
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.AddAsync(It.IsAny<Book>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task BooksService_DeleteByIdAsync_DeletesBook()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.DeleteByIdAsync(It.IsAny<int>()));
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            //Act
            await bookService.DeleteByIdAsync(1);
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.DeleteByIdAsync(1), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }
        
        [Test]
        public async Task BooksService_UpdateAsync_UpdatesBook()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.BookRepository.Update(It.IsAny<Book>()));
            var bookService = new BooksService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            
            //Act
            await bookService.UpdateAsync(new BookModel());
            
            //Assert
            mockUnitOfWork.Verify(x => x.BookRepository.Update(It.IsAny<Book>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }


        private IEnumerable<BookModel> GetTestBookModels()
        {
            return new List<BookModel>()
            {
                new BookModel(){ Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996},
                new BookModel(){ Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994}
            };
        }
    }
}