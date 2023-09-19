using Microsoft.EntityFrameworkCore;
using SmartWhere.Sample.DomainObject.Entity;

namespace SmartWhere.Sample.Api.ApplicationSpecific.Contexts
{
    public class MemoryContext : DbContext
    {
        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder.UseInMemoryDatabase("DummyDataDb"));
            }
        }
    }
}
