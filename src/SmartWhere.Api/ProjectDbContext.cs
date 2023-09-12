using Microsoft.EntityFrameworkCore;
using SmartWhere.Sample.Api.Entities;
using System.Reflection;

namespace SmartWhere.Sample.Api
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
