using SmartWhere.Sample.Api.ApplicationSpecific.Contexts;
using SmartWhere.Sample.DomainObject.Entity;

namespace SmartWhere.Sample.Api.ApplicationSpecific
{
    public class PublisherRepository
    {
        private readonly MemoryContext _context = new();

        public void AddRange(IEnumerable<Publisher> publishers)
        {
            _context.AddRange(publishers);
            _context.SaveChanges();
        }
    }
}
