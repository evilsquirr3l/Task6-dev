using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        private LibraryDbContext _context;
        private DbContextOptions<LibraryDbContext> _options;
        
        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(_options))
            {
                context.Books.Add(new Book(){ Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996});
                context.Cards.Add(new Card(){ Id = 1, ReaderId = 1, Created = DateTime.Now});
                context.Readers.Add(new Reader(){Id = 1, Email = "jon_snow@epam.com", Name = "Jon Snow"});
                context.ReaderProfiles.Add(new ReaderProfile(){ Id = 1, ReaderId = 1, Address = "The night's watch", Phone = "golub"});
                context.Histories.Add(new History(){BookId = 1, CardId = 1, Id = 1, TakeDate = DateTime.Now.AddDays(-2), ReturnDate = DateTime.Now.AddDays(-1)});
                context.SaveChanges();
            }
        }

        [Test]
        public void BookRepository_FindAll_ReturnsAllValues()
        {
            using (var context = new LibraryDbContext(_options))
            {
                var booksRepository = new Repository<Book>(context);

                var books = booksRepository.FindAll().ToList();

                Assert.AreEqual(1, books.Count);
            }
        }

        [Test]
        public void BookRepository_GetById_ReturnsSingleValue()
        {
            using (var context = new LibraryDbContext(_options))
            {
                var booksRepository = new Repository<Book>(context);

                var book = booksRepository.FindByCondition(g => g.Id == 1).SingleOrDefault();

                Assert.AreEqual(1, book.Id);
                Assert.AreEqual("Jon Snow", book.Author);
                Assert.AreEqual("A song of ice and fire", book.Title);
                Assert.AreEqual(1996, book.Year);
            }
        }

        [Test]
        public async Task BookRepository_Delete_DeletesEntity()
        {
            using (var context = new LibraryDbContext(_options))
            {
                var bookRepository = new Repository<Book>(context);
                
                await bookRepository.DeleteById(1);
                await context.SaveChangesAsync();
                
                Assert.AreEqual(0, context.Books.Count());
            }
        }
    }
}