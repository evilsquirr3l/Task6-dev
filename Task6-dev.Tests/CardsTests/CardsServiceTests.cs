using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace Task6.CardsTests
{
    [TestFixture]
    public class CardsServiceTests
    {
        [Test]
        public void CardsService_GetAll_ReturnsCardModels()
        {
            var expected = GetTestCardModels();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.CardRepository.FindAllWithDetails())
                .Returns(GetTestCardEntities().AsQueryable);
            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actual = cardService.GetAll().ToList();

            Assert.That(actual, Is.EqualTo(expected).Using(new CardModelEqualityComparer()));
        }

        private List<CardModel> GetTestCardModels()
        {
            return new List<CardModel>()
            {
                new CardModel {Id = 1, Created = DateTime.Today.AddHours(2), ReaderId = 1 },
                new CardModel {Id = 2, Created = DateTime.Today.AddHours(4), ReaderId = 2 },
                new CardModel {Id = 3, Created = DateTime.Today.AddHours(6), ReaderId = 1 },
                new CardModel {Id = 4, Created = DateTime.Today.AddHours(8), ReaderId = 2 }
            };
        }

        private List<Card> GetTestCardEntities()
        {
            return new List<Card>()
            {
                new Card {Id = 1, Created = DateTime.Today.AddHours(2), ReaderId = 1 },
                new Card {Id = 2, Created = DateTime.Today.AddHours(4), ReaderId = 2 },
                new Card {Id = 3, Created = DateTime.Today.AddHours(6), ReaderId = 1 },
                new Card {Id = 4, Created = DateTime.Today.AddHours(8), ReaderId = 2 }
            };
        }

        [Test]
        public async Task CardsService_GetByIdAsync_ReturnsCardModel()
        {
            var expected = GetTestCardModels().First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.CardRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestCardEntities().First);
            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actual = await cardService.GetByIdAsync(1);

            Assert.That(actual, Is.EqualTo(expected).Using(new CardModelEqualityComparer()));
        }

        [Test]
        public async Task CardsService_AddAsync_AddsModel()
        {
            //Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CardRepository.AddAsync(It.IsAny<Card>()));
            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var card = new CardModel { Id = 100, Created = DateTime.Today };

            //Act
            await cardService.AddAsync(card);

            //Assert
            mockUnitOfWork.Verify(x => x.CardRepository.AddAsync(It.Is<Card>(c => c.Created == card.Created && c.Id == card.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(-5)]
        public async Task CardsService_DeleteByIdAsync_DeletesCard(int cardId)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.CardRepository.DeleteByIdAsync(It.IsAny<int>()));
            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            await cardService.DeleteByIdAsync(cardId);

            mockUnitOfWork.Verify(x => x.CardRepository.DeleteByIdAsync(cardId), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task CardsService_UpdateAsync_UpdatesCard()
        {
            //Arrange
            var card = new CardModel { Id = 1, Created = DateTime.Today };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CardRepository.Update(It.IsAny<Card>()));
            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Act
            await cardService.UpdateAsync(card);

            //Assert
            mockUnitOfWork.Verify(x => x.CardRepository.Update(It.Is<Card>(c => c.Created == card.Created && c.Id == card.Id)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase(1)]
        public async Task CardsService_GetBooksByCardIdAsync_ReturnsCorrectBooks(int cardId)
        {
            //Arrange
            var expected = GetTestBookModels().Where(b => b.CardsIds.Contains(cardId)).ToList();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(x => x.CardRepository.GetByIdWithDetailsAsync(It.IsAny<int>())).ReturnsAsync(() => GetTestCardWithHistoryById(cardId));
            mockUnitOfWork.Setup(x => x.BookRepository.FindAllWithDetails()).Returns(() => GetTestBooksWithHistoryByCardId(cardId));

            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Act
            var books = await cardService.GetBooksByCardIdAsync(cardId);
            var actual = books.ToList();

            //Assert
            Assert.That(actual, Is.EqualTo(expected).Using(new BookModelEqualityComparer()));
        }

        private IEnumerable<BookModel> GetTestBookModels()
        {
            return new List<BookModel>()
            {
                new BookModel {Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909, CardsIds = new List<int> { 1 } },
                new BookModel {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994, CardsIds = new List<int> { 2 }},
                new BookModel {Id = 3, Author = "Jack London", Title = "The Call of the Wild", Year = 1903, CardsIds = new List<int> { 2 }},
                new BookModel {Id = 4, Author = "Robert Jordan", Title = "Lord of Chaos", Year = 1994}
            };
        }

        private IQueryable<Book> GetTestBooksWithHistoryByCardId(int cardId)
        {
            var books = new List<Book>()
            {
                new Book
                {
                    Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909,
                    Cards = new List<History> { new History { CardId = 1} }
                },
                new Book
                {
                    Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994,
                    Cards = new List<History> { new History { CardId = 2} }
                },
                new Book
                {
                    Id = 3, Author = "Jack London", Title = "The Call of the Wild", Year = 1903,
                    Cards = new List<History> { new History { CardId = 2} }
                }
            };

            return books.Where(x => x.Cards.Any(c => c.CardId == cardId)).AsQueryable();
        }

        private Card GetTestCardWithHistoryById(int cardId)
        {
            var cards = new List<Card>()
            {
                new Card
                {
                    Id = 1,
                    Created = DateTime.Today.AddHours(2),
                    ReaderId = 1,
                    Books = new List<History>
                    {
                        new History { Book = new Book { Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909 } }
                    }
                },
                new Card
                {
                    Id = 2,
                    Created = DateTime.Today.AddHours(4),
                    ReaderId = 2,
                    Books = new List<History>
                    {
                        new History { Book = new Book {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994} },
                        new History { Book = new Book {Id = 3, Author = "Jack London", Title = "The Call of the Wild", Year = 1903} }
                    }
                },
                new Card {Id = 3, Created = DateTime.Today.AddHours(6), ReaderId = 1 },
            };

            return cards.FirstOrDefault(x => x.Id == cardId);
        }

        [Test]
        public async Task CardsService_TakeBookAsync_CreatesHistoryWhereBookIsSignedToCard()
        {
            //Arrange
            var book = new Book { Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909 };
            var card = new Card { Id = 1, Created = DateTime.Today, ReaderId = 1 };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CardRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(card);
            mockUnitOfWork.Setup(x => x.BookRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            mockUnitOfWork.Setup(x => x.HistoryRepository.FindAll());

            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Act
            await cardService.TakeBookAsync(1, 1);

            //Assert

            //What if he uses AddAsync?
            var result = card.Books.Count == 1
                || book.Cards.Count == 1;

            Assert.IsTrue(result);
        }

        [Test]
        public void CardsSercice_TakeUnexistingBook_ThrowsExeption()
        {
            //Arrange
            var card = new Card { Id = 1, Created = DateTime.Today, ReaderId = 1 };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CardRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(card);
            mockUnitOfWork.Setup(x => x.BookRepository.GetByIdAsync(It.IsAny<int>()));
            mockUnitOfWork.Setup(x => x.HistoryRepository.FindAll());

            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Act/Assert
            Assert.ThrowsAsync<LibraryException>(async () => await cardService.TakeBookAsync(1, 1));
        }

        [Test]
        public void CardsSercice_TakeBookToUnexistingCard_ThrowsExeption()
        {
            //Arrange
            var book = new Book { Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909 };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CardRepository.GetByIdAsync(It.IsAny<int>()));
            mockUnitOfWork.Setup(x => x.BookRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            mockUnitOfWork.Setup(x => x.HistoryRepository.FindAll());

            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Act/Assert
            Assert.ThrowsAsync<LibraryException>(async () => await cardService.TakeBookAsync(1, 1));
        }

        [Test]
        public void CardsSercice_TakeUnreturnedBook_ThrowsExeption()
        {
            //Arrange
            var book = new Book { Id = 1, Author = "Jack London", Title = "Martin Eden", Year = 1909 };
            var card = new Card { Id = 1, Created = DateTime.Today, ReaderId = 1 };
            var history = new History { BookId = book.Id, Book = book, TakeDate = DateTime.Now, ReturnDate = DateTime.Today.AddDays(5) };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CardRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(card);
            mockUnitOfWork.Setup(x => x.BookRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            mockUnitOfWork.Setup(x => x.HistoryRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(history);
            mockUnitOfWork.Setup(x => x.HistoryRepository.FindAll()).Returns(Enumerable.Repeat(history, 1).AsQueryable());

            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Act/Assert
            Assert.ThrowsAsync<LibraryException>(async () => await cardService.TakeBookAsync(1, 1));
        }

        private IEnumerable<History> GetHistories()
        {
            return new List<History>
            {
                new History { Id = 1, BookId = 1, CardId = 1, TakeDate = DateTime.Today },
                new History { Id = 2, BookId = 2, CardId = 1, TakeDate = DateTime.Today },
            };
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public async Task CardsService_HandOverBookAsync_ReturnDateOfHistoryWasChanged(int cardId, int bookId)
        {
            //Arrange
            var histories = GetHistories();

            var history = histories.FirstOrDefault(x => x.BookId == bookId && x.CardId == cardId);
            var expected = new History { Id = history.Id, BookId = history.BookId, CardId = history.CardId, TakeDate = history.TakeDate };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.HistoryRepository.FindAll()).Returns(histories.AsQueryable());
 
            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Act
            await cardService.HandOverBookAsync(cardId, bookId);
            var actual = history;

            //Assert
            Assert.AreNotEqual(expected.ReturnDate, actual.ReturnDate);
            mockUnitOfWork.Verify(x => x.HistoryRepository.Update(It.Is<History>(x => x.BookId == bookId && x.CardId == cardId)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase(4, 1)]
        [TestCase(1, 5)]
        [TestCase(5, 6)]
        public void CardsService_HandOverBookAsync_ThrowsExceptionIfHistoryWasNotFound(int cardId, int bookId)
        {
            //Arrange
            var histories = GetHistories();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.HistoryRepository.FindAll()).Returns(histories.AsQueryable());

            var cardService = new CardService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Act/Assert
            Assert.ThrowsAsync<LibraryException>(async () => await cardService.HandOverBookAsync(cardId, bookId));
        }
    }
}
