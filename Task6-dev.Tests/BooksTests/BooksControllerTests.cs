using System.Collections.Generic;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace Task6.BooksTests
{
    [TestFixture]
    public class BooksControllerTests
    {
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
    }
}