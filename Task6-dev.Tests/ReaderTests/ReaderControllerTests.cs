using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace Task6.ReaderTests
{
    [TestFixture]
    public class ReaderControllerTests
    {
        private DbContextOptions<LibraryDbContext> _options;
        private ReaderModelEqualityComparer _readerModelComparer = new ReaderModelEqualityComparer();

        [SetUp]
        public void Setup()
        {
            _options = UnitTestHelper.GetUnitTestDbOptions();
        }

        [Test]
        public void ReaderController_GetAllReaders_ReturnsReadersModels()
        {
            //Arrange
            var expected = GetTestReaderModels().ToList();
            var mockReaderService = new Mock<IReaderService>();
            mockReaderService
                .Setup(x => x.GetAll())
                .Returns(GetTestReaderModels());
            var readerController = new ReaderController(mockReaderService.Object);

            //Act
            var result = readerController.Get();

            //Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<ReaderModel>>>(result);
            var actual = result.Value.ToList();
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count(); i++)
            {
                var expectedReaderModel = expected[i];
                var actualReaderModel = actual[i];
                Assert.IsTrue(_readerModelComparer.Equals(expectedReaderModel, actualReaderModel));
            }
        }

        [Test]
        public async Task ReaderController_GetById_ReturnsReaderModelById()
        {
            //Arrange
            var expected = GetTestReaderModels().First();
            int readerId = 1;
            var mockReaderService = new Mock<IReaderService>();
            mockReaderService
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestReaderModels().First());
            var readerController = new ReaderController(mockReaderService.Object);

            //Act
            var result = await readerController.Get(readerId);

            //Assert
            Assert.IsInstanceOf<ActionResult<ReaderModel>>(result);
            var actual = result.Value;
            Assert.IsTrue(_readerModelComparer.Equals(expected, actual));
        }

        [Test]
        public async Task ReaderController_GetById_ReturnsNotFound()
        {
            //Arrange
            int readerId = 999;
            var mockReaderService = new Mock<IReaderService>();
            var readerController = new ReaderController(mockReaderService.Object);

            //Act
            var actual = await readerController.Get(readerId);

            //Assert
            Assert.IsInstanceOf<ActionResult<ReaderModel>>(actual);
            Assert.IsInstanceOf<NotFoundObjectResult>(actual.Result);
        }

        [Test]
        public void ReaderController_GetReadersThatDontReturnBooks_ReturnsReadersModels()
        {
            //Arrange
            var expected = GetTestReaderModels().ToList();
            var mockReaderService = new Mock<IReaderService>();
            mockReaderService
                .Setup(x => x.GetReadersThatDontReturnBooks())
                .Returns(GetTestReaderModels());
            var readerController = new ReaderController(mockReaderService.Object);

            //Act
            var result = readerController.GetReadersThatDontReturnBooks();

            //Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<ReaderModel>>>(result);
            var actual = result.Value.ToList();
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count(); i++)
            {
                var expectedReaderModel = expected[i];
                var actualReaderModel = actual[i];
                Assert.IsTrue(_readerModelComparer.Equals(expectedReaderModel, actualReaderModel));
            }
        }

        //Show post method, and discuss about put and delete
        [Test]
        public async Task ReaderController_Post_ReturnCreatedAtAction()
        {
            //Arrange
            var newReader = new ReaderModel()
            {
                Id = 10,
                Name = "Test_Adding",
                Email = "test_adding_email@gmail.com",
                Phone = "123789456",
                Address = "Kyiv, 10"
            };
            var mockReaderService = new Mock<IReaderService>();
            mockReaderService.Setup(x => x.AddAsync(It.IsAny<ReaderModel>()));
            var readerController = new ReaderController(mockReaderService.Object);

            //Act
            var actual = await readerController.Post(newReader);

            //Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(actual);
            Assert.NotNull(actual);
            //Assert.IsTrue(_readerModelComparer.Equals(newReader, actual.Value));
        }

        [Test]
        public async Task ReaderController_Put_ReturnNoContent()
        {
            //Arrange
            var newReader = new ReaderModel()
            {
                Id = 10,
                Name = "Test_Update",
                Email = "test_update_email@gmail.com",
                Phone = "123789456",
                Address = "Kyiv, 10"
            };
            var mockReaderService = new Mock<IReaderService>();
            mockReaderService.Setup(x => x.UpdateAsync(It.IsAny<ReaderModel>()));
            var readerController = new ReaderController(mockReaderService.Object);

            //Act
            var actual = await readerController.Put(newReader);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(actual);   
        }

        [Test]
        public async Task ReaderController_Delete_ReturnCreatedAtAction()
        {
            //Arrange
            var id = 1;
            var mockReaderService = new Mock<IReaderService>();
            mockReaderService.Setup(x => x.DeleteByIdAsync(It.IsAny<int>()));
            var readerController = new ReaderController(mockReaderService.Object);

            //Act
            var actual = await readerController.Delete(id);

            //Assert
            Assert.IsInstanceOf<OkResult>(actual);
        }

        #region data for tests
        private IEnumerable<ReaderModel> GetTestReaderModels()
        {
            return new List<ReaderModel>()
            {
                new ReaderModel { Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com",
                    Phone = "123456789", Address = "Kyiv, 1" },
                new ReaderModel { Id = 2, Name = "Ivan", Email = "ivan_email@gmail.com",
                    Phone = "456789123", Address = "Kyiv, 2" },
                new ReaderModel { Id = 3, Name = "Petro", Email = "petro_email@gmail.com",
                    Phone = "789123456", Address = "Kyiv, 3" },
                new ReaderModel { Id = 4, Name = "Oleksandr", Email = "oleksandr_email@gmail.com",
                    Phone = "326159487", Address = "Kyiv, 4" }
            };
        }
        #endregion
    }
}
