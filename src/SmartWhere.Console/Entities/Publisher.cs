namespace SmartWhere.Console.Entities
{
    public class Publisher
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}
