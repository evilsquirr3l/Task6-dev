using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public LibraryDbContext()
        {
            //Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<History> Histories { get; set; }

        public DbSet<Reader> Readers { get; set; }

        public DbSet<ReaderProfile> ReaderProfiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=libraryappdb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }


    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reader>().HasData(
                new Reader { Id = 1, Name = "Serhii", Email = "serhii_email@gmail.com" },
                new Reader { Id = 2, Name = "Ivan", Email = "ivan_email@gmail.com" },
                new Reader { Id = 3, Name = "Petro", Email = "petro_email@gmail.com" },
                new Reader { Id = 4, Name = "Oleksandr", Email = "oleksandr_email@gmail.com" }
            );
            modelBuilder.Entity<ReaderProfile>().HasData(
                new ReaderProfile { Id = 1, ReaderId = 1, Phone = "123456789", Address = "Kyiv, 1" },
                new ReaderProfile { Id = 2, ReaderId = 2, Phone = "456789123", Address = "Kyiv, 2" },
                new ReaderProfile { Id = 3, ReaderId = 3, Phone = "789123456", Address = "Kyiv, 3" },
                new ReaderProfile { Id = 4, ReaderId = 4, Phone = "326159487", Address = "Kyiv, 4" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "The Picture of Dorian Gray", Year = 1890, Author = "Oscar Wilde" },
                new Book { Id = 2, Title = "White fang", Year = 1906, Author = "Jack London" },
                new Book { Id = 3, Title = "Robinson Crusoe", Year = 1719, Author = "Daniel Defo" },
                new Book { Id = 4, Title = "The Old Man and the Sea", Year = 1952, Author = "Ernest Hemingway" },
                new Book { Id = 5, Title = "A Dance with Dragons", Year = 2011, Author = "George R. R. Martin" }
            );
            modelBuilder.Entity<Card>().HasData(
                new Card { Id = 1, ReaderId = 1, Created = new DateTime(2016, 7, 20) },
                new Card { Id = 2, ReaderId = 2, Created = new DateTime(2017, 10, 2) },
                new Card { Id = 3, ReaderId = 3, Created = new DateTime(2018, 2, 15) },
                new Card { Id = 4, ReaderId = 4, Created = new DateTime(2016, 8, 5) },
                new Card { Id = 5, ReaderId = 4, Created = new DateTime(2020, 7, 6) }
            );

            modelBuilder.Entity<History>().HasData(
                new History { Id = 1, BookId = 1, CardId = 1, TakeDate = new DateTime(2016, 7, 20), ReturnDate = new DateTime(2016, 8, 20) },
                new History { Id = 2, BookId = 2, CardId = 4, TakeDate = new DateTime(2016, 8, 5), ReturnDate = new DateTime(2016, 8, 20) },
                new History { Id = 3, BookId = 3, CardId = 4, TakeDate = new DateTime(2016, 5, 5), ReturnDate = new DateTime(2016, 8, 30) },
                new History { Id = 4, BookId = 2, CardId = 1, TakeDate = new DateTime(2018, 11, 16), ReturnDate = new DateTime(2018, 12, 29) },
                new History { Id = 5, BookId = 1, CardId = 2, TakeDate = new DateTime(2020, 5, 19), ReturnDate = new DateTime(2020, 6, 15) },
                new History { Id = 6, BookId = 3, CardId = 1, TakeDate = new DateTime(2020, 6, 5) },
                new History { Id = 7, BookId = 4, CardId = 3, TakeDate = new DateTime(2020, 6, 28) },
                new History { Id = 8, BookId = 5, CardId = 5, TakeDate = new DateTime(2020, 7, 6) }
            );
        }
    }
}