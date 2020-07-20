using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Business;
using Business.Models;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Task6
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<LibraryDbContext> GetUnitTestDbOptions()
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

        public static Mapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        public static void SeedData(LibraryDbContext context)
        {
            context.Books.Add(new Book {Id = 1, Author = "Jon Snow", Title = "A song of ice and fire", Year = 1996});
            context.Books.Add(new Book {Id = 2, Author = "John Travolta", Title = "Pulp Fiction", Year = 1994});
            context.Cards.Add(new Card {Id = 1, ReaderId = 1, Created = DateTime.Now});
            context.Readers.Add(new Reader {Id = 1, Email = "jon_snow@epam.com", Name = "Jon Snow"});
            context.ReaderProfiles.Add(new ReaderProfile {Id = 1, ReaderId = 1, Address = "The night's watch", Phone = "golub"});
            context.Histories.Add(new History {BookId = 1, CardId = 1, Id = 1, TakeDate = DateTime.Now.AddDays(-2), ReturnDate = DateTime.Now.AddDays(-1)});
                
            context.SaveChanges();
        }
    }
}