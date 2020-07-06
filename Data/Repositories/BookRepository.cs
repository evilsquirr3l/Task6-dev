using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context)
        {
        }

        public IQueryable<Book> GetAllWithDetails()
        {
            return FindAll().Include(b => b.Cards);
        }

        public async Task<Book> GetByIdWithDetailsAsync(int id)
        {
            return await FindAll()
                .Include(b => b.Cards)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}