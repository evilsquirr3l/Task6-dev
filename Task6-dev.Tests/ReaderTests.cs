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

namespace Task6
{
    [TestFixture]
    public class ReaderTests
    {
        private DbContextOptions<LibraryDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = UnitTestHelper.SeedData();
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
                Assert.AreEqual(1, readers.Count());
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

        //TODO:  GetReadersThatDontReturnBooks

        [Test]
        public void ReaderService_GetAll_ReturnsReaderModels()
        {
            //arrange
            var expected = GetTestReaderModels().ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.ReaderRepository.GetAllWithDetails())
                .Returns(GetTestReaderEntities());
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = readerService.GetAll().ToList();

            //assert
            Assert.IsInstanceOf<IEnumerable<ReaderModel>>(actual);
            Assert.AreEqual(expected[0].Name, actual[0].Name);
            Assert.AreEqual(expected[0].Email, actual[0].Email);
            Assert.AreEqual(expected[1].Phone, actual[1].Phone);
            Assert.AreEqual(expected[1].Address, actual[1].Address);
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
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Phone, actual.Phone);
            Assert.AreEqual(expected.Address, actual.Address);
        }

        [Test]
        public async Task ReaderService_AddAsync_AddModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.AddAsync(It.IsAny<Reader>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            await readerService.AddAsync(new ReaderModel(){Id = 10, Name = "Petro", 
                Email = "petro_email@gmail.com", Phone = "789123456", Address = "Kyiv, 3"
            });

            //assert
            mockUnitOfWork.Verify(x => x.ReaderRepository.AddAsync(It.IsAny<Reader>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task ReaderService_UpdateAsync_UpdateModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.Update(It.IsAny<Reader>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            await readerService.UpdateAsync(1, new ReaderModel());

            //assert
            mockUnitOfWork.Verify(x => x.ReaderRepository.Update(It.IsAny<Reader>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task ReaderService_DeleteAsync_DeleteModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.ReaderRepository.DeleteById(It.IsAny<int>()));
            var readerService = new ReaderService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            await readerService.DeleteAsync(1);

            //assert
            mockUnitOfWork.Verify(x => x.ReaderRepository.DeleteById(It.IsAny<int>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        #endregion

        #region controller
        [Test]
        public void ReaderController_GetAllReaders_ReturnsReadersModels()
        {
            //Arrange
            var mockReaderService = new Mock<IReaderService>();
            mockReaderService
                .Setup(x => x.GetAll())
                .Returns(GetTestReaderModels());
            var readerController = new ReaderController(mockReaderService.Object);

            //Act
            var result = readerController.Get();
            var values = result.Result as OkObjectResult;

            //Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<ReaderModel>>>(result);
            Assert.NotNull(values.Value);
        }
        //TODO: ask about ok() and propose to make like in msdn
        #endregion

        #region data for tests
        private IEnumerable<ReaderModel> GetTestReaderModels()
        {
            return new List<ReaderModel>()
            {
                new ReaderModel(){ Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com",
                    Phone = "123456789", Address = "Kyiv, 1" },
                new ReaderModel(){ Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com",
                    Phone = "123456789", Address = "Kyiv, 1" },
            };
        }
        private IQueryable<Reader> GetTestReaderEntities()
        {
            return new List<Reader>()
            {
                new Reader(){ Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com",
                    ReaderProfile = new ReaderProfile{ ReaderId = 1, Phone = "123456789", Address = "Kyiv, 1" } },
                new Reader(){ Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com",
                    ReaderProfile = new ReaderProfile{ ReaderId = 2, Phone = "123456789", Address = "Kyiv, 1" } },
            }.AsQueryable();
        }
        #endregion
    }
}
