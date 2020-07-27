using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Task6.CardsTests
{
    [TestFixture]
    public class CardsRepositoryTests
    {
        [Test]
        public async Task CardRepository_GetByIdWithDetailsAsync_ReturnsCardByIdAndIncludesBooks()
        {
            //Arrange
            using var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var cardRepository = new CardRepository(context);
            //TODO: discuss to upgrade expected values
            var expectedCard = context.Cards.AsNoTracking().Include(x => x.Books).Include(x => x.Reader).FirstOrDefault(x => x.Id == 1);

            //Act
            var card = await cardRepository.GetByIdWithDetailsAsync(1);

            //Assert
            Assert.That(card, Is.EqualTo(expectedCard).Using(new CardEqualityComparer()));
            Assert.That(card.Books, Is.EqualTo(expectedCard.Books).Using(new HistoryEqualityComparer()));
            Assert.That(card.Reader, Is.EqualTo(expectedCard.Reader).Using(new ReaderEqualityComparer()));
        }

        [Test]
        public async Task CardRepository_GetAllWithDetails_ReturnsCardByIdAndIncludesBooks()
        {
            //Arrange
            using var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var cardRepository = new CardRepository(context);
            //TODO: discuss to upgrade expected values
            var expectedCards = context.Cards.AsNoTracking().Include(x => x.Books).Include(x => x.Reader).ToList();

            //Act
            var cards = cardRepository.GetAllWithDetails().ToList();

            //Assert
            Assert.That(cards, Is.EqualTo(expectedCards).Using(new CardEqualityComparer()));
            for (int i = 0; i < cards.Count; i++)
            {
                Assert.That(cards[i].Books, Is.EqualTo(expectedCards[i].Books).Using(new HistoryEqualityComparer()));
                Assert.That(cards[i].Reader, Is.EqualTo(expectedCards[i].Reader).Using(new ReaderEqualityComparer()));
            }
        }
    }
}
