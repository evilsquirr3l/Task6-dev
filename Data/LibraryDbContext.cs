using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<ReaderProfile>()
            //     .HasOne(rp => rp.Reader)
            //     .WithOne(r => r.ReaderProfile)
            //     .HasForeignKey(s => s.)
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<History> Histories { get; set; }

        public DbSet<Reader> Readers { get; set; }

        public DbSet<ReaderProfile> ReaderProfiles { get; set; }
    }
}