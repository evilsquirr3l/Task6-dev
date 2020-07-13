using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Business;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Task6
{
    internal static class UnitTestHelper
    {
        public static IEqualityComparer<Card> CardEqualityComparer = new CardEqualityComparer();

        public static IEqualityComparer<Book> BookEqualityComparer = new BookEqualityComparer();

        public static IEqualityComparer<History> HistoryEqualityComparer = new HistoryEqualityComparer();

        public static DbContextOptions<LibraryDbContext> SeedData()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                context.Books.Add(new Book {Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996});
                context.Cards.Add(new Card {Id = 1, ReaderId = 1, Created = DateTime.Now});
                context.Readers.Add(new Reader {Id = 1, Email = "jon_snow@epam.com", Name = "Jon Snow"});
                context.ReaderProfiles.Add(new ReaderProfile {Id = 1, ReaderId = 1, Address = "The night's watch", Phone = "golub"});
                context.Histories.Add(new History {BookId = 1, CardId = 1, Id = 1, TakeDate = DateTime.Now.AddDays(-2), ReturnDate = DateTime.Now.AddDays(-1)});
                
                context.SaveChanges();
            }

            return options;
        }

        public static Mapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

    }

    internal class CardEqualityComparer : IEqualityComparer<Card>
    {
        public bool Equals([AllowNull] Card x, [AllowNull] Card y)
        {
            return x.Id == y.Id
                && x.Created == y.Created
                && x.ReaderId == y.ReaderId;
        }

        public int GetHashCode([DisallowNull] Card obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class HistoryEqualityComparer : IEqualityComparer<History>
    {
        public bool Equals([AllowNull] History x, [AllowNull] History y)
        {
            return x.Id == y.Id
                && x.BookId == y.BookId
                && x.CardId == y.CardId
                && x.TakeDate == y.TakeDate
                && x.ReturnDate == y.ReturnDate;
        }

        public int GetHashCode([DisallowNull] History obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class BookEqualityComparer : IEqualityComparer<Book>
    {
        public bool Equals([AllowNull] Book x, [AllowNull] Book y)
        {
            return x.Id == y.Id
                && x.Year == y.Year
                && x.Author == y.Author;
        }

        public int GetHashCode([DisallowNull] Book obj)
        {
            return obj.GetHashCode();
        }
    }
}