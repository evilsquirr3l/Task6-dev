using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }
        
        IRepository<Card> CardRepository { get; }
        
        IHistoryRepository HistoryRepository { get; }
        
        IRepository<Reader> ReaderRepository { get; }

        Task<int> SaveAsync();
    }
}