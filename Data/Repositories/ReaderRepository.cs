using System;
using Data.Interfaces;
using Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReaderRepository : Repository<Reader>, IReaderRepository
    {
        public ReaderRepository(LibraryDbContext context) : base(context)
        {
        }

        public IQueryable<Reader> GetAllWithDetails()
        {
            return FindAll()
                .Include(x => x.ReaderProfile)
                .Include(x => x.Card);
        }

        public async Task<Reader> GetByIdWithDetails(int id)
        {
            return await FindAll()
                .Include(x => x.ReaderProfile)
                .Include(x => x.Card)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
