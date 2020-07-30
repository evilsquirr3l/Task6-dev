using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Business.Models;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Library.Tests.IntegrationTests
{
    [TestFixture]
    public class CardsIntegrationTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory _factory;
        private const string RequestUri = "api/cards/";
        private IEqualityComparer<CardModel> _cardModelComparer;
        private IEqualityComparer<BookModel> _bookModelComparer;

        [SetUp]
        public void Init()
        {
            _cardModelComparer = new CardModelEqualityComparer();
            _bookModelComparer = new BookModelEqualityComparer();
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        private List<CardModel> GetCardsModels()
        {
            return new List<CardModel>
            {
                new CardModel { Id = 1, ReaderId = 1, Created = new DateTime(2020, 7, 21) },
                new CardModel { Id = 2, ReaderId = 2, Created = new DateTime(2020, 7, 23) }
            };
        }

        [Test, Order(0)]
        public async Task CardsController_GetAll_ReturnsAllCardsFromDb()
        {
            // arrange 
            var expected = GetCardsModels();

            // act
            var httpResponse = await _client.GetAsync(RequestUri);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<CardModel>>(stringResponse).ToList();
            Assert.That(actual, Is.EqualTo(expected).Using(_cardModelComparer));
        }

        [Test, Order(0)]
        public async Task CardsController_GetById_ReturnsCardFromDb()
        {
            // arrange 
            var expected = GetCardsModels().First();
            var cardId = 1;

            // act
            var httpResponse = await _client.GetAsync(RequestUri + cardId);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<CardModel>(stringResponse);
            Assert.That(actual, Is.EqualTo(expected).Using(_cardModelComparer));
        }

        [Test, Order(1)]
        public async Task CardsController_Update_UpdatesBookInDatabase()
        {
            var expected = new CardModel { Id = 2, ReaderId = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(RequestUri, content);

            httpResponse.EnsureSuccessStatusCode();

            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();
                var actual = await context.Cards.FindAsync(expected.Id);

                Assert.AreEqual(expected.ReaderId, actual.ReaderId);
                Assert.AreEqual(expected.Created, actual.Created);

                Assert.AreEqual(2, context.Cards.Count());
            }
        }

        [Test, Order(2)]
        public async Task CardsController_DeleteById_DeletesCardFromDatabase()
        {
            var cardId = 1;
            var httpResponse = await _client.DeleteAsync(RequestUri + cardId);

            httpResponse.EnsureSuccessStatusCode();

            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<LibraryDbContext>();

                Assert.AreEqual(1, context.Cards.Count());
            }
        }

        [Test, Order(1)]
        public async Task CardsController_Add_AddsCardToDb()
        {
            //Arrange
            var card = new CardModel { ReaderId = 1 };
            var cardId = 3;
            var content = new StringContent(JsonConvert.SerializeObject(card), Encoding.UTF8, "application/json");

            //Act
            var httpResponse = await _client.PostAsync(RequestUri, content);
            httpResponse.EnsureSuccessStatusCode();

            //Assert
            using var test = _factory.Services.CreateScope();

            var context = test.ServiceProvider.GetService<LibraryDbContext>();
            var actual = context.Cards.Find(cardId);

            Assert.AreEqual(3, context.Cards.Count());

            Assert.AreEqual(card.ReaderId, actual.ReaderId);
            Assert.AreEqual(card.Created, actual.Created);
        }

        [Test, Order(0)]
        public async Task CardsController_GetBooksByCardId_ReturnsBooksFromDatabaseByCardId()
        {
            var cardId = 1;
            var books = new List<BookModel> { new BookModel { Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996 } };

            var httpResponse = await _client.GetAsync(RequestUri + cardId + "/books");
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<BookModel>>(stringResponse).ToList();
            Assert.That(actual, Is.EqualTo(books).Using(_bookModelComparer));
        }

        [Test, Order(1)]
        public async Task CardsController_TakeBook_CreatesHistoryWithCardAndBookIds()
        {
            var cardId = 1;
            var bookId = 2;
            var createdHistoryId = 3;

            var httpResponse = await _client.PostAsync(RequestUri + cardId + "/books/" + bookId, content: null);
            httpResponse.EnsureSuccessStatusCode();

            using var test = _factory.Services.CreateScope();
            var context = test.ServiceProvider.GetService<LibraryDbContext>();

            var history = context.Histories.Find(createdHistoryId);
            Assert.AreEqual(history.BookId, bookId);
            Assert.AreEqual(history.CardId, cardId);
        }

        [Test, Order(1)]
        public async Task CardsController_HandOverBook_UpdatesReturnDateInHistory()
        {
            var cardId = 2;
            var bookId = 2;
            var historyId = 2;

            var httpResponse = await _client.DeleteAsync(RequestUri + cardId + "/books/" + bookId);
            httpResponse.EnsureSuccessStatusCode();

            using var test = _factory.Services.CreateScope();
            var context = test.ServiceProvider.GetService<LibraryDbContext>();

            var history = context.Histories.Find(historyId);

            Assert.IsNotNull(history.ReturnDate);
        }

        [Test, Order(0)]
        public async Task CardsController_HandOverBook_ReturnsBadRequestIfLibraryExceptionWasThrown()
        {
            var cardId = 6;
            var bookId = 2;

            var httpResponse = await _client.DeleteAsync(RequestUri + cardId + "/books/" + bookId);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test, Order(0)]
        public async Task CardsController_TakeBook_ReturnsBadRequestIfLibraryExceptionWasThrown()
        {
            var cardId = 6;
            var bookId = 2;

            var httpResponse = await _client.PostAsync(RequestUri + cardId + "/books/" + bookId, content: null);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
