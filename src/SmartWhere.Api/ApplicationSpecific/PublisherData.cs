using SmartWhere.Sample.DomainObject.Entity;

namespace SmartWhere.Sample.Api.ApplicationSpecific
{
    public static class PublisherData
    {
        private static readonly List<string> Countries = new()
        {
            "Germany",
            "England",
            "USA",
            "Turkey",
            "China",
            "India",
            "Cyprus",
            "Greece",
            "Bulgaria",
            "Italy",
            "France",
            "Spain"
        };
        public static void FillDummyData()
        {
            PublisherRepository publisherRepository = new();

            List<Publisher> publishers = new();

            var index = 1;

            for (var i = 1; i <= 5; i++)
            {
                Publisher publisher = new()
                {
                    Id = i,
                    Name = $"Publisher {i}",
                    Books = new List<Book>()
                };

                for (var j = 1; j <= 5; j++)
                {
                    var birthday = DateTime.Now.AddYears(new Random().Next(j, j * 3) * -1);

                    Book book = new()
                    {
                        Id = index,
                        Name = $"Book {index}",
                        Price = 10.5 * j,
                        PublishedYear = DateTime.Now.AddYears(new Random().Next(j, j * 10) * -1).Year,
                        Author = new Author
                        {
                            Id = index,
                            Name = $"Name {index}",
                            Surname = $"Surname {index}",
                            Birthday = birthday,
                            Age = DateTime.Now.Year - birthday.Year,
                            Country = new Country
                            {
                                Id = index,
                                Name = Countries[new Random().Next(0, Countries.Count - 1)]
                            }
                        }
                    };

                    publisher.Books.Add(book);

                    index++;
                }

                publishers.Add(publisher);
            }

            publisherRepository.AddRange(publishers);
        }
    }
}
