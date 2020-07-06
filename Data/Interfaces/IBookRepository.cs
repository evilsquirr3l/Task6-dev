using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IQueryable<Book> GetAllWithDetails();

        Task<Book> GetByIdWithDetails(int id);
    }
}