namespace SmartWhere.Sample.Api.ApplicationSpecific
{
    public class PublisherRepository
    {
        private readonly MemoryContext _context = new();

        public void AddRange(IEnumerable<Publisher.Entities.Publisher> publishers)
        {
            _context.AddRange(publishers);
            _context.SaveChanges();
        }
    }
}
