using Sample.Common.Entity;

namespace Sample.Common;

public static class DataInitializer
{
    private static readonly List<string> Countries =
    [
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
        "Spain",
        "Hungary"
    ];

    private static readonly List<(int publisherId, int index, string bookName, string authorName, int publishedYear)> BookAndAuthorNames =
    [
        (1, 1, "In Search of Lost Time", "Marcel Proust", 2020),
        (1, 2, "Ulysses", "James Joyce", 1986),
        (1, 3, "Don Quixote", "Miguel de Cervantes", 1956),
        (1, 4, "One Hundred Years of Solitude", "Gabriel Garcia Marquez", 1898),
        (1, 5, "The Great Gatsby", "F. Scott Fitzgerald", 2000),
        (1, 6, "Moby Dick", "Herman Melville", 1900),
        (1, 7, "War and Peace", "Leo Tolstoy", 1992),
        (1, 8, "Hamlet", "William Shakespeare", 2021),
        (1, 9, "The Odyssey", "Homer", 1978),
        (1, 10, "Madame Bovary", "Gustave Flaubert", 2020),
        (2, 11, "The Divine Comedy", "Dante Alighieri", 2001),
        (2, 12, "Lolita", "Vladimir Nabokov", 1987),
        (2, 13, "The Brothers Karamazov", "Fyodor Dostoyevsky", 1970),
        (2, 14, "Crime and Punishment", "Fyodor Dostoyevsky", 1999),
        (2, 15, "Wuthering Heights", "Emily Brontë", 1907),
        (2, 16, "The Catcher in the Rye", "J. D. Salinger", 1980),
        (2, 17, "Pride and Prejudice", "Jane Austen", 2012),
        (2, 18, "The Adventures of Huckleberry Finn", "Mark Twain", 2001),
        (2, 19, "Anna Karenina", "Leo Tolstoy", 2003),
        (2, 20, "Alice's Adventures in Wonderland", "Lewis Carroll", 1954),
        (3, 21, "The Iliad", "Homer", 1961),
        (3, 22, "To the Lighthouse", "Virginia Woolf", 1999),
        (3, 23, "Catch-22", "Joseph Heller", 1986),
        (3, 24, "Heart of Darkness", "Joseph Conrad", 1988),
        (3, 25, "The Sound and the Fury", "William Faulkner", 1990),
        (3, 26, "Nineteen Eighty Four", "George Orwell", 1991),
        (3, 27, "Great Expectations", "Charles Dickens", 1948),
        (3, 28, "One Thousand and One Nights", "India/Iran/Iraq/Egypt", 1938),
        (3, 29, "The Grapes of Wrath", "John Steinbeck", 1881),
        (3, 30, "Absalom, Absalom!", "William Faulkner", 1901),
        (4, 31, "Invisible Man", "Ralph Ellison", 1898),
        (4, 32, "To Kill a Mockingbird", "Harper Lee", 2004),
        (4, 33, "The Trial", "Franz Kafka", 2005),
        (4, 34, "The Red and the Black", "Stendhal", 1999),
        (4, 35, "Middlemarch", "George Eliot", 1970),
        (4, 36, "Gulliver's Travels", "Jonathan Swift", 1980),
        (4, 37, "Mrs. Dalloway", "Virginia Woolf", 1990),
        (4, 38, "The Stories of Anton Chekhov", "Anton Chekhov", 1999),
        (4, 39, "The Stranger", "Albert Camus", 1908),
        (4, 40, "Jane Eyre", "Charlotte Bronte", 1991),
        (5, 41, "The Aeneid", "Virgil", 2000),
        (5, 42, "Collected Fiction", "Jorge Luis Borges", 1985),
        (5, 43, "The Sun Also Rises", "Ernest Hemingway", 1986),
        (5, 44, "David Copperfield", "Charles Dickens", 1988),
        (5, 45, "Tristram Shandy", "Laurence Sterne", 1994),
        (5, 46, "Leaves of Grass", "Walt Whitman", 2000),
        (5, 47, "The Magic Mountain", "Thomas Mann", 1955),
        (5, 48, "A Portrait of the Artist as a Young Man", "James Joyce", 1948),
        (5, 49, "Midnight's Children", "Salman Rushdie", 1997),
        (5, 50, "Beloved", "Toni Morrison", 2023)
    ];

    private static readonly List<Publisher> Publishers = [];

    public static IQueryable<Publisher> FillMockData()
    {

        if (Publishers.Any())
            return Publishers.AsQueryable();

        var index = 1;

        for (var i = 1; i <= 5; i++)
        {
            Publisher publisher = new()
            {
                Id = i,
                Name = $"Publisher {i}"
            };

            var publisherBooks = BookAndAuthorNames.Where(x => x.publisherId == i);

            foreach (var publisherBook in publisherBooks)
            {
                Book book = new()
                {
                    Id = publisherBook.index,
                    Name = publisherBook.bookName,
                    Price = new Random().NextDouble() * (323.2 - 10.5) + 10.5,
                    PublishedYear = publisherBook.publishedYear,
                    CreatedDate = DateTime.Now.AddDays(-index),
                    Author = new Author
                    {
                        Id = index,
                        Name = publisherBook.authorName,
                        Age = index == 3 ? 30 : new Random().Next(20, 65),
                        Country = new Country
                        {
                            Id = index,
                            Name = Countries[new Random().Next(0, Countries.Count)]
                        }
                    }
                };

                publisher.Books.Add(book);

                index++;
            }

            Publishers.Add(publisher);
        }


        return Publishers.AsQueryable();
    }
}