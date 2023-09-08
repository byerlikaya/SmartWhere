namespace SmartWhere.Console.Entities
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Test> Test { get; set; }
    }

    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
