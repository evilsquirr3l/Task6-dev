using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }
        
        IRepository<Card> CardRepository { get; }
        
        IRepository<History> HistoryRepository { get; }
        
        IReaderRepository ReaderRepository { get; }

        Task<int> SaveAsync();
    }
}