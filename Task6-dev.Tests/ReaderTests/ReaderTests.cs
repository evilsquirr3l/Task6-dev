using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace Task6.Tests.ReaderTests
{
    [TestFixture]
    public class ReaderTests
    {
        private DbContextOptions<LibraryDbContext> _options;
        private ReaderModelComparer _readerModelComparer = new ReaderModelComparer();

        [SetUp]
        public void Setup()
        {
            _options = UnitTestHelper.GetDbContextOptions();
        }

        #region repository
        [Test]
        public void ReaderRepository_GetAllWithDetails_ReturnAllValues()
        {
            using (var context = new LibraryDbContext(_options))
            {
                //arrange
                var readerRepository = new ReaderRepository(context);

                //act
                var readers = readerRepository.GetAllWithDetails().ToList();

                //assert
                Assert.AreEqual(2, readers.Count());
                Assert.IsNotNull(readers[0].ReaderProfile);
                Assert.AreEqual("The night's watch", readers[0].ReaderProfile.Address);
                Assert.AreEqual("golub", readers[0].ReaderProfile.Phone);
            }
        }

        [Test]
        public async Task ReaderRepository_GetByIdWithDetails_ReturnValueById()
        {
            using (var context = new LibraryDbContext(_options))
            {
                //arrange
                int id = 1;
                var readerRepository = new ReaderRepository(context);

                //act
                var reader = await readerRepository.GetByIdWithDetails(id);

                //assert
                Assert.IsNotNull(reader);
                Assert.AreEqual("Jon Snow", reader.Name);
                Assert.AreEqual("jon_snow@epam.com", reader.Email);
                Assert.AreEqual("The night's watch", reader.ReaderProfile.Address);
                Assert.AreEqual("golub", reader.ReaderProfile.Phone);
            }
        }
        #endregion

        #region service

        [Test]
        public void ReaderService_GetAll_ReturnsReaderModels()
        {
            //arrange
            var expected = GetTestReaderModels().ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.ReaderRepository.GetAllWithDetails())
                .Returns(GetTestReaderEntities().AsQueryable());
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = readerService.GetAll().ToList();

            //assert
            Assert.IsInstanceOf<IEnumerable<ReaderModel>>(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count(); i++)
            {
                var expectedReaderModel = expected[i];
                var actualReaderModel = actual[i];
                Assert.IsTrue(_readerModelComparer.Equals(expectedReaderModel, actualReaderModel));
            }
        }

        [Test]
        public async Task ReaderService_GetByIdAsync_ReturnsReaderModels()
        {
            //arrange
            var expected = GetTestReaderModels().First();
            var id = 1;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.ReaderRepository.GetByIdWithDetails(It.IsAny<int>()))
                .ReturnsAsync(GetTestReaderEntities().First());
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await readerService.GetByIdAsync(id);

            //assert
            Assert.IsInstanceOf<ReaderModel>(actual);
            Assert.IsTrue(_readerModelComparer.Equals(expected, actual));
        }

        [Test]
        public async Task ReaderService_AddAsync_AddModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.AddAsync(It.IsAny<Reader>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var newReader = new ReaderModel()
            {
                Id = 10,
                Name = "Test_Adding",
                Email = "test_adding_email@gmail.com",
                Phone = "123789456",
                Address = "Kyiv, 10"
            };

            //act
            await readerService.AddAsync(newReader);

            //assert
            mockUnitOfWork.Verify(x => x.ReaderRepository.AddAsync(
                It.Is<Reader>(r => r.Id == newReader.Id && r.Name == newReader.Name
                && r.Email == newReader.Email && r.ReaderProfile.Phone == newReader.Phone 
                && r.ReaderProfile.Address == newReader.Address)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task ReaderService_UpdateAsync_UpdateModel()
        {
            //arrange
            var reader = new ReaderModel()
            {
                Id = 10,
                Name = "Test_Updating",
                Email = "test_adding_email@gmail.com"
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.Update(It.IsAny<Reader>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            await readerService.UpdateAsync(reader);

            //assert
            mockUnitOfWork.Verify(x => x.ReaderRepository.Update(
                It.Is<Reader>(r => r.Id == reader.Id && r.Name == reader.Name && r.Email == reader.Email)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task ReaderService_DeleteByIdAsync_DeleteModel()
        {
            //arrange
            int readerId = 1;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.DeleteByIdAsync(It.IsAny<int>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            await readerService.DeleteByIdAsync(readerId);

            //assert
            mockUnitOfWork.Verify(x => x.ReaderRepository.DeleteByIdAsync(readerId), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void ReaderService_GetReadersThatDontReturnBooks_ReaderModels()
        {
            //arrange
            var expected = GetTestReaderModels().ToList();
            expected.RemoveAt(expected.Count - 1);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.ReaderRepository.GetAllWithDetails())
                .Returns(GetTestReaderEntities().AsQueryable());
            mockUnitOfWork
                .Setup(m => m.HistoryRepository.FindAll())
                .Returns(GetTestHistoryEntities().AsQueryable());
            mockUnitOfWork
                .Setup(m => m.CardRepository.FindAll())
                .Returns(GetTestCardEntities().AsQueryable());                
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = readerService.GetReadersThatDontReturnBooks().ToList();

            //assert
            Assert.IsInstanceOf<IEnumerable<ReaderModel>>(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count(); i++)
            {
                var expectedReaderModel = expected[i];
                var actualReaderModel = actual[i];
                Assert.IsTrue(_readerModelComparer.Equals(expectedReaderModel, actualReaderModel));
            }
        }

        #endregion

        #region controller
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

        #endregion

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

        private IEnumerable<Reader> GetTestReaderEntities()
        {
            return new List<Reader>()
            {
                 new Reader { Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com",
                    ReaderProfile = new ReaderProfile { ReaderId = 1, Phone = "123456789", Address = "Kyiv, 1" }},
                 new Reader { Id = 2, Name = "Ivan", Email = "ivan_email@gmail.com",
                    ReaderProfile = new ReaderProfile { ReaderId = 2, Phone = "456789123", Address = "Kyiv, 2" }},
                 new Reader { Id = 3, Name = "Petro", Email = "petro_email@gmail.com",
                    ReaderProfile = new ReaderProfile { ReaderId = 3, Phone = "789123456", Address = "Kyiv, 3" }},
                 new Reader { Id = 4, Name = "Oleksandr", Email = "oleksandr_email@gmail.com",
                    ReaderProfile = new ReaderProfile { ReaderId = 4, Phone = "326159487", Address = "Kyiv, 4" }}
            };
        }

        private IEnumerable<Card> GetTestCardEntities()
        {
            return new List<Card>()
            {
                new Card { Id = 1, ReaderId = 1, Created = DateTime.Now.AddDays(-10) },
                new Card { Id = 2, ReaderId = 2, Created = DateTime.Now.AddDays(-7) },
                new Card { Id = 3, ReaderId = 3, Created = DateTime.Now.AddDays(-3) },
                new Card { Id = 4, ReaderId = 4, Created = DateTime.Now },
            };
        }
        private IEnumerable<History> GetTestHistoryEntities()
        {
            return new List<History>()
            {
                new History { Id = 1, BookId = 1, CardId = 1, TakeDate = DateTime.Now.AddDays(-10), ReturnDate = DateTime.Now.AddDays(-7) },
                new History { Id = 2, BookId = 2, CardId = 2, TakeDate = DateTime.Now.AddDays(-7), ReturnDate = DateTime.Now.AddDays(-1) },
                new History { Id = 3, BookId = 3, CardId = 2, TakeDate = DateTime.Now.AddDays(-7), ReturnDate = DateTime.Now.AddDays(-4) },
                new History { Id = 4, BookId = 2, CardId = 3, TakeDate = DateTime.Now.AddDays(-3), ReturnDate = DateTime.Now },
                new History { Id = 5, BookId = 1, CardId = 2, TakeDate = DateTime.Now.AddDays(-7) },
                new History { Id = 6, BookId = 3, CardId = 1, TakeDate = DateTime.Now.AddDays(-3) },
                new History { Id = 7, BookId = 4, CardId = 3, TakeDate = DateTime.Now.AddDays(-3) }
            };
        }
        #endregion
    }
}
