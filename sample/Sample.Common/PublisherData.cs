using Sample.Common.Entity;

namespace Sample.Common
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
            "Spain",
            "Hungary"
        };

        private static readonly List<(int index, string bookName, string authorName, int publishedYear)> BookAndAuthorNames = new()
        {
           (1,"In Search of Lost Time","Marcel Proust",2020),
           (2,"Ulysses","James Joyce",1986),
           (3,"Don Quixote","Miguel de Cervantes",1956),
           (4,"One Hundred Years of Solitude","Gabriel Garcia Marquez",1898),
           (5,"The Great Gatsby","F. Scott Fitzgerald",2000),
           (6,"Moby Dick","Herman Melville",1900),
           (7,"War and Peace","Leo Tolstoy",1992),
           (8,"Hamlet","William Shakespeare",2021),
           (9,"The Odyssey","Homer",1978),
           (10,"Madame Bovary","Gustave Flaubert",2020),
           (11,"The Divine Comedy","Dante Alighieri",2001),
           (12,"Lolita","Vladimir Nabokov",1987),
           (13,"The Brothers Karamazov","Fyodor Dostoyevsky",1970),
           (14,"Crime and Punishment","Fyodor Dostoyevsky",1999),
           (15,"Wuthering Heights","Emily Brontë",1907),
           (16,"The Catcher in the Rye","J. D. Salinger",1980),
           (17,"Pride and Prejudice","Jane Austen",2012),
           (18,"The Adventures of Huckleberry Finn","Mark Twain",2001),
           (19,"Anna Karenina","Leo Tolstoy",2003),
           (20,"Alice's Adventures in Wonderland","Lewis Carroll",1954),
           (21,"The Iliad","Homer",1961),
           (22,"To the Lighthouse","Virginia Woolf",1999),
           (23,"Catch-22","Joseph Heller",1986),
           (24,"Heart of Darkness","Joseph Conrad",1988),
           (25,"The Sound and the Fury","William Faulkner",1990),
           (26,"Nineteen Eighty Four","George Orwell",1991),
           (27,"Great Expectations","Charles Dickens",1948),
           (28,"One Thousand and One Nights","India/Iran/Iraq/Egypt",1938),
           (29,"The Grapes of Wrath","John Steinbeck",1881),
           (30,"Absalom, Absalom!","William Faulkner",1901),
           (31,"Invisible Man","Ralph Ellison",1898),
           (32,"To Kill a Mockingbird","Harper Lee",2004),
           (33,"The Trial","Franz Kafka",2005),
           (34,"The Red and the Black","Stendhal",1999),
           (35,"Middlemarch","George Eliot",1970),
           (36,"Gulliver's Travels","Jonathan Swift",1980),
           (37,"Mrs. Dalloway","Virginia Woolf",1990),
           (38,"The Stories of Anton Chekhov","Anton Chekhov",1999),
           (39,"The Stranger","Albert Camus",1908),
           (40,"Jane Eyre","Charlotte Bronte",1991),
           (41,"The Aeneid","Virgil",2000),
           (42,"Collected Fiction","Jorge Luis Borges",1985),
           (43,"The Sun Also Rises","Ernest Hemingway",1986),
           (44,"David Copperfield","Charles Dickens",1988),
           (45,"Tristram Shandy","Laurence Sterne",1994),
           (46,"Leaves of Grass","Walt Whitman",2000),
           (47,"The Magic Mountain","Thomas Mann",1955),
           (48,"A Portrait of the Artist as a Young Man","James Joyce",1948),
           (49,"Midnight's Children","Salman Rushdie",1997),
           (50,"Beloved","Toni Morrison",2023)
        };

        private static readonly List<Publisher> Publishers = new();

        public static IQueryable<Publisher> FillDummyData()
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

                for (var j = 1; j <= 10; j++)
                {
                    var randomBookIndex = GetRandomIndex();
                    var randomBook = BookAndAuthorNames.FirstOrDefault(x => x.index == randomBookIndex);

                    Book book = new()
                    {
                        Id = index,
                        Name = randomBook.bookName,
                        Price = new Random().NextDouble() * (323.2 - 10.5) + 10.5,
                        PublishedYear = randomBook.publishedYear,
                        Author = new Author
                        {
                            Id = index,
                            Name = randomBook.authorName,
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

        private static readonly List<int> RandomIndexList = new();

        private static int GetRandomIndex()
        {
            while (true)
            {
                var index = new Random().Next(0, BookAndAuthorNames.Count);
                if (RandomIndexList.Any(x => x == index)) continue;

                RandomIndexList.Add(index);
                return index;
            }
        }
    }
}
