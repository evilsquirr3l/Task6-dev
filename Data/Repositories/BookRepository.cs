using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class BookRepository : Repository<Book>
    {
        public BookRepository(LibraryDbContext context) : base(context)
        {
        }

        public override IQueryable<Book> FindAll()
        {
            return DbSet.Include(b => b.Cards);
        }

        public override async Task<Book> GetByIdAsync(int id)
        {
            return await FindAll()
                .Include(b => b.Cards)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}