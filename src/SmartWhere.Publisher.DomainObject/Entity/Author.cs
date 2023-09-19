namespace SmartWhere.Sample.DomainObject.Entity
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public int Age { get; set; }

        public Country Country { get; set; }
    }
}
