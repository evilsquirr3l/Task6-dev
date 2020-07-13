using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public UnitOfWork(LibraryDbContext context, IRepository<Card> cardRepository, IHistoryRepository historyRepository, IRepository<Reader> readerRepository, IBookRepository bookRepository)
        {
            _context = context;
            CardRepository = cardRepository;
            HistoryRepository = historyRepository;
            ReaderRepository = readerRepository;
            BookRepository = bookRepository;
        }

        public IBookRepository BookRepository { get; }
        public IRepository<Card> CardRepository { get; }
        public IHistoryRepository HistoryRepository { get; }
        public IRepository<Reader> ReaderRepository { get; }
        
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}