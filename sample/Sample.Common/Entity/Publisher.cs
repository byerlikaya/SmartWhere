namespace Sample.Common.Entity
{
    public class Publisher
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Book> Books { get; set; } = new();
    }
}
