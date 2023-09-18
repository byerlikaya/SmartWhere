using Microsoft.EntityFrameworkCore;
using SmartWhere.Northwind.Entities;

namespace SmartWhere.Sample.Api.ApplicationSpecific
{
    public class MSSqlContext : DbContext
    {
        private IConfiguration Configuration { get; }

        public MSSqlContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
            }
        }
    }
}
