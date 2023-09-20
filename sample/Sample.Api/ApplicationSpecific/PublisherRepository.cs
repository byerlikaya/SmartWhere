using Sample.Api.ApplicationSpecific.Contexts;
using Sample.Common.Entity;

namespace Sample.Api.ApplicationSpecific
{
    public class PublisherRepository
    {
        private readonly MemoryDbContext _context = new();

        public void AddRange(IEnumerable<Publisher> publishers)
        {
            _context.AddRange(publishers);
            _context.SaveChanges();
        }
    }
}
