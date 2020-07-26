using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card> GetByIdWithBooksAsync(int id);
    }
}
