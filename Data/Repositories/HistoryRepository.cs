using System.Linq;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class HistoryRepository : Repository<History>
    {
        public HistoryRepository(LibraryDbContext context) : base(context) { }

        public override IQueryable<History> FindAll()
        {
            return _dbSet
                .Include(h => h.Book)
                .Include(h => h.Card)
                    .ThenInclude(c => c.Reader);
        }
    }
}