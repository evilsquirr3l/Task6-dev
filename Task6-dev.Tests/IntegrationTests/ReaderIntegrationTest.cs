using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi;
using Data;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Business.Models;
using System.Text;

namespace Task6.IntegrationTests
{
    [TestFixture]
    public class ReaderIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private ReaderModelEqualityComparer _comparer;
        private string requestUri = "api/reader/";

        [OneTimeSetUp]
        public void Init()
        {
            _comparer = new ReaderModelEqualityComparer();
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task ReaderController_GetAll_ReturnAllFromDb()
        {
            // arrange 
            var expected = GetReaderModels().ToList();
            var expectedLength = expected.Count;

            // act
            var httpResponse = await _client.GetAsync(requestUri);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<ReaderModel>>(stringResponse).ToList();
            Assert.AreEqual(expectedLength, actual.Count);
            for (int i = 0; i < expectedLength; i++)
            {
                Assert.IsTrue(_comparer.Equals(expected[i], actual[i]));
            }
        }

        [Test]
        public async Task ReaderController_GetByIdAsync_ReturnOneReader()
        {
            // arrange 
            var expected = GetReaderModels().First();
            var readerId = 1;

            // act
            var httpResponse = await _client.GetAsync(requestUri + readerId);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<ReaderModel>(stringResponse);
            Assert.IsTrue(_comparer.Equals(expected, actual));
        }

        [Test]
        public async Task ReaderController_Post_AddReaderToDb()
        {
            // arrange
            var reader = new ReaderModel { Name = "Scrooge McDuck", Email = "only_money@gmail.com", 
                Phone = "999999999", Address = "Glasgow" };
            var readerId = 3;
            var content = new StringContent(JsonConvert.SerializeObject(reader), Encoding.UTF8, "application/json");
            
            // act
            var httpResponse = await _client.PostAsync(requestUri, content);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            CheckReaderInfoIntoDb(reader, readerId, 3);
        }

        //TODO: test for validation

        [Test]
        public async Task ReaderController_Put_UpdateReaderInDb()
        {
            // arrange
            var reader = new ReaderModel
            {
                Id = 2,
                Name = "Enzo Ferrari",
                Email = "scuderia_ferrari@gmail.com",
                Phone = "165479823",
                Address = "Modena, Maranello"
            };
            var content = new StringContent(JsonConvert.SerializeObject(reader), Encoding.UTF8, "application/json");

            // act
            var httpResponse = await _client.PutAsync(requestUri, content);

            //assert
            httpResponse.EnsureSuccessStatusCode();
            CheckReaderInfoIntoDb(reader, reader.Id, 2);
        }

        [Test]
        public async Task ReaderController_Delete_DeleteReaderFromDb()
        {
            // arrange
            var readerId = 2;

            // act
            var httpResponse = await _client.DeleteAsync(requestUri + readerId);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();

                Assert.AreEqual(2, context.Readers.Count());
                Assert.AreEqual(2, context.ReaderProfiles.Count());
            }
        }

        private async void CheckReaderInfoIntoDb(ReaderModel reader, int readerId, int expectedLength)
        {
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();
                Assert.AreEqual(expectedLength, context.Readers.Count());
                Assert.AreEqual(expectedLength, context.ReaderProfiles.Count());

                var dbReader = await context.Readers.FindAsync(readerId);
                Assert.AreEqual(readerId, dbReader.Id);
                Assert.AreEqual(reader.Name, dbReader.Name);
                Assert.AreEqual(reader.Email, dbReader.Email);

                var dbReaderProfile = await context.ReaderProfiles.FindAsync(readerId);
                Assert.AreEqual(readerId, dbReaderProfile.ReaderId);
                Assert.AreEqual(reader.Phone, dbReaderProfile.Phone);
                Assert.AreEqual(reader.Address, dbReaderProfile.Address);
            }
        }

        private IEnumerable<ReaderModel> GetReaderModels()
        {
            return new List<ReaderModel>()
            {
                new ReaderModel { Id = 1, Name = "Jon Snow", Email = "jon_snow@epam.com",
                    Phone = "golub", Address = "The night's watch" },
                new ReaderModel { Id = 2, Name = "Night King", Email = "night_king@gmail.com",
                    Phone = "telepathy", Address = "North" }
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
