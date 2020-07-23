using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
