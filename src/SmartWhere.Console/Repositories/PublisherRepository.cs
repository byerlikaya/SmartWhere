using Microsoft.EntityFrameworkCore;
using SmartWhere.Console.Entities;

namespace SmartWhere.Console.Repositories
{
    public class PublisherRepository
    {
        private readonly ProjectDbContext _context = new();

        public void AddRange(IEnumerable<Publisher> publishers)
        {
            _context.AddRange(publishers);
            _context.SaveChanges();
        }

        public IQueryable<Publisher> PublisherQuery() => _context.Set<Publisher>()
            .Include(x => x.Books)
            .ThenInclude(x => x.Author)
            .AsNoTracking();
    }
}
