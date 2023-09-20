using Microsoft.EntityFrameworkCore;
using Sample.Common.Entity;
using System.Reflection;

namespace Sample.Api.ApplicationSpecific.Contexts
{
    public class MemoryDbContext : DbContext
    {
        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder.UseInMemoryDatabase("DummyDataDb"));
            }
        }
    }
}
