using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SmartWhere.Sample.Api.ApplicationSpecific
{
    public class MemoryContext : DbContext
    {
        public DbSet<Publisher.Entities.Publisher> Publishers { get; set; }

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
