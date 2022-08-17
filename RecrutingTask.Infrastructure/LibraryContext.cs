using Microsoft.EntityFrameworkCore;
using RecrutingTask.Domain.Models;

namespace RecrutingTask.Infrastructure
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RecrutingTask;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.LogTo(System.Console.WriteLine);
        }

    }
}
