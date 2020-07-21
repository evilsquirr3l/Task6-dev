using System.Linq;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    public interface IHistoryRepository : IRepository<History>
    {
        IQueryable<History> GetAllWithDetails();
    }
}