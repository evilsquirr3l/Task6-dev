using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CardRepository : Repository<Card>, ICardRepository
    {
        public CardRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Card> GetByIdWithBooksAsync(int id)
        {
            return await DbSet.Include(x => x.Books)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
