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
        public async Task CardRepository_GetByIdWithBooksAsync_ReturnsCardByIdAndIncludesBooks()
        {
            using (var context = new LibraryDbContext(UnitTestHelper.GetUnitTestDbOptions()))
            {
                var cardRepository = new CardRepository(context);

                //TODO: discuss to upgrade expected values
                var expectedCard = context.Cards.AsNoTracking().Include(x => x.Books).FirstOrDefault(x => x.Id == 1);

                var card = await cardRepository.GetByIdWithDetailsAsync(1);

                Assert.That(card, Is.EqualTo(expectedCard).Using(new CardEqualityComparer()));
                Assert.That(card.Books, Is.EqualTo(expectedCard.Books).Using(new HistoryEqualityComparer()));
            }
        }
    }
}
