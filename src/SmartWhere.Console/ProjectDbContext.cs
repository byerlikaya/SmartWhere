using Microsoft.EntityFrameworkCore;
using SmartWhere.Console.Entities;
using System.Reflection;

namespace SmartWhere.Console
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<Publisher> Publishers { get; set; }

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
