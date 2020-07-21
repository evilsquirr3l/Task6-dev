using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public UnitOfWork(LibraryDbContext context, ICardRepository cardRepository, IRepository<History> historyRepository, IRepository<Reader> readerRepository, IBookRepository bookRepository)
        {
            _context = context;
            CardRepository = cardRepository;
            HistoryRepository = historyRepository;
            ReaderRepository = readerRepository;
            BookRepository = bookRepository;
        }

        public IBookRepository BookRepository { get; }
        public ICardRepository CardRepository { get; }
        public IRepository<History> HistoryRepository { get; }
        public IRepository<Reader> ReaderRepository { get; }
        
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}