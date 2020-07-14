using System;
using AutoMapper;
using Business;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Task6
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<LibraryDbContext> GetDbContextOptions()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new LibraryDbContext(options))
            {
                SeedData(context);
            }

            return options;
        }

        public static void SeedData(LibraryDbContext context)
        {
            context.Books.Add(new Book { Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996 });
            context.Cards.Add(new Card { Id = 1, ReaderId = 1, Created = DateTime.Now });
            context.Readers.Add(new Reader { Id = 1, Email = "jon_snow@epam.com", Name = "Jon Snow" });
            context.ReaderProfiles.Add(new ReaderProfile { ReaderId = 1, Address = "The night's watch", Phone = "golub" });
            context.Readers.Add(new Reader { Id = 2, Email = "night_king@epam.com", Name = "Night King" });
            context.ReaderProfiles.Add(new ReaderProfile { ReaderId = 2, Address = "North", Phone = "none" });
            context.Histories.Add(new History { BookId = 1, CardId = 1, Id = 1, TakeDate = DateTime.Now.AddDays(-2), ReturnDate = DateTime.Now.AddDays(-1) });

            context.SaveChanges();
        }

        public static Mapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}