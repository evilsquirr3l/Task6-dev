using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class HistoryRepository : Repository<History>, IHistoryRepository
    {
        public HistoryRepository(LibraryDbContext context) : base(context) { }


        public IQueryable<History> GetAllWithDetails()
        {
            return FindAll()
                .Include(h => h.Book)
                .Include(h => h.Card)
                .ThenInclude(c => c.Reader);
        }

        public async Task<History> GetByIdWithDetailsAsync(int id)
        {
            return await GetAllWithDetails().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}