using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public UnitOfWork(LibraryDbContext context, IRepository<Book> bookRepository, IRepository<Card> cardRepository, IRepository<History> historyRepository, IRepository<Reader> readerRepository)
        {
            _context = context;
            BookRepository = bookRepository;
            CardRepository = cardRepository;
            HistoryRepository = historyRepository;
            ReaderRepository = readerRepository;
        }

        public IRepository<Book> BookRepository { get; }
        public IRepository<Card> CardRepository { get; }
        public IRepository<History> HistoryRepository { get; }
        public IRepository<Reader> ReaderRepository { get; }
        
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}