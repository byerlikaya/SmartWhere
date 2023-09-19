using SmartWhere.Sample.Common.Entity;

namespace SmartWhere.Sample.Common
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

        private static readonly List<(int index, string bookName, string authorName)> BookAndAuthorNames = new()
        {
           (1,"In Search of Lost Time","Marcel Proust"),
           (2,"Ulysses","James Joyce"),
           (3,"Don Quixote","Miguel de Cervantes"),
           (4,"One Hundred Years of Solitude","Gabriel Garcia Marquez"),
           (5,"The Great Gatsby","F. Scott Fitzgerald"),
           (6,"Moby Dick","Herman Melville"),
           (7,"War and Peace","Leo Tolstoy"),
           (8,"Hamlet","William Shakespeare"),
           (9,"The Odyssey","Homer"),
           (10,"Madame Bovary","Gustave Flaubert"),
           (11,"The Divine Comedy","Dante Alighieri"),
           (12,"Lolita","Vladimir Nabokov"),
           (13,"The Brothers Karamazov","Fyodor Dostoyevsky"),
           (14,"Crime and Punishment","Fyodor Dostoyevsky"),
           (15,"Wuthering Heights","Emily Brontë"),
           (16,"The Catcher in the Rye","J. D. Salinger"),
           (17,"Pride and Prejudice","Jane Austen"),
           (18,"The Adventures of Huckleberry Finn","Mark Twain"),
           (19,"Anna Karenina","Leo Tolstoy"),
           (20,"Alice's Adventures in Wonderland","Lewis Carroll"),
           (21,"The Iliad","Homer"),
           (22,"To the Lighthouse","Virginia Woolf"),
           (23,"Catch-22","Joseph Heller"),
           (24,"Heart of Darkness","Joseph Conrad"),
           (25,"The Sound and the Fury","William Faulkner"),
           (26,"Nineteen Eighty Four","George Orwell"),
           (27,"Great Expectations","Charles Dickens"),
           (28,"One Thousand and One Nights","India/Iran/Iraq/Egypt"),
           (29,"The Grapes of Wrath","John Steinbeck"),
           (30,"Absalom, Absalom!","William Faulkner"),
           (31,"Invisible Man","Ralph Ellison"),
           (32,"To Kill a Mockingbird","Harper Lee"),
           (33,"The Trial","Franz Kafka"),
           (34,"The Red and the Black","Stendhal"),
           (35,"Middlemarch","George Eliot"),
           (36,"Gulliver's Travels","Jonathan Swift"),
           (37,"Mrs. Dalloway","Virginia Woolf"),
           (38,"The Stories of Anton Chekhov","Anton Chekhov"),
           (39,"The Stranger","Albert Camus"),
           (40,"Jane Eyre","Charlotte Bronte"),
           (41,"The Aeneid","Virgil"),
           (42,"Collected Fiction","Jorge Luis Borges"),
           (43,"The Sun Also Rises","Ernest Hemingway"),
           (44,"David Copperfield","Charles Dickens"),
           (45,"Tristram Shandy","Laurence Sterne"),
           (46,"Leaves of Grass","Walt Whitman"),
           (47,"The Magic Mountain","Thomas Mann"),
           (48,"A Portrait of the Artist as a Young Man","James Joyce"),
           (49,"Midnight's Children","Salman Rushdie"),
           (50,"Beloved","Toni Morrison")
        };

        public static IQueryable<Publisher> FillDummyData()
        {

            List<Publisher> publishers = new();

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
                        PublishedYear = DateTime.Now.AddYears(new Random().Next(j, j * 10) * -1).Year,
                        Author = new Author
                        {
                            Id = index,
                            Name = randomBook.authorName,
                            Age = new Random().Next(20, 65),
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


            return publishers.AsQueryable();
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
