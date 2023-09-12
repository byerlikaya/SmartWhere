using SmartWhere.Sample.Api.Entities;
using SmartWhere.Sample.Api.Repositories;

namespace SmartWhere.Sample.Api
{
    public static class PublisherData
    {
        public static void FillDummyData()
        {
            PublisherRepository publisherRepository = new();
            publisherRepository.AddRange(new List<Publisher>
            {
                new Publisher
                {
                    Id = 1,
                    Name = "Publisher 1",
                    Books = new List<Book>
                    {
                        new Book
                        {
                            Id = 1,
                            Name = "Book 1",
                            Author = new Author
                            {
                                Id = 1,
                                Name = "Author 1",
                                Surname = "Author 1"
                            }
                        },
                        new Book
                        {
                            Id = 2,
                            Name = "Book 2",
                            Author = new Author
                            {
                                Id = 2,
                                Name = "Author 2",
                                Surname = "Author 2"
                            }
                        }
                    }
                },
                new Publisher
                {
                    Id = 2,
                    Name = "Publisher 2",
                    Books = new List<Book>
                    {
                        new Book
                        {
                            Id = 3,
                            Name = "Book 3",
                            Author = new Author
                            {
                                Id =3,
                                Name = "Author 3",
                                Surname = "Author 3"
                            }
                        },
                        new Book
                        {
                            Id = 4,
                            Name = "Book 4",
                            Author = new Author
                            {
                                Id = 4,
                                Name = "Author 4",
                                Surname = "Author 4"
                            }
                        },
                        new Book
                        {
                            Id = 5,
                            Name = "Book 5",
                            Author = new Author
                            {
                                Id = 5,
                                Name = "Author 5",
                                Surname = "Author 5"
                            }
                        }
                    }
                }
            });
        }
    }
}