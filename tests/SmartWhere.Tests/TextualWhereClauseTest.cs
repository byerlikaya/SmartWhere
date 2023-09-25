namespace SmartWhere.Test
{
    public class TextualWhereClauseTest
    {
        private readonly IQueryable<Book> _books = DataInitializer.FillMockData().SelectMany(x => x.Books).AsQueryable();

        [Fact]
        public void SmartWhere_Should_Return_Results_Contains_Name_Parameter()
        {
            //Arrange
            TextualSearchRequest request = new()
            {
                Name = "Nineteen"
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Name.Contains("Nineteen")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Not_Contains_BookName_Parameter()
        {
            //Arrange
            TextualSearchRequest request = new()
            {
                BookName = "Example Book Name"
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => !x.Name.Contains("Example Book Name")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Starting_With_Name_Parameter()
        {
            //Arrange
            BookSearchRequest request = new()
            {
                Start = 0,
                Max = 50,
                SearchData = new BookSearchDto
                {
                    Name = "Nineteen"
                }
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Name.StartsWith("Nineteen")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Not_Starting_With_AuthorName_Parameter()
        {
            //Arrange
            TextualSearchRequest request = new()
            {
                AuthorName = "Camus"
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => !x.Name.StartsWith("Camus")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Ending_With_AuthorName_Parameter()
        {
            //Arrange
            BookSearchRequest request = new()
            {
                Start = 0,
                Max = 50,
                SearchData = new BookSearchDto
                {
                    AuthorName = "Camus"
                }
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Author.Name.EndsWith("Camus")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Not_Ending_With_CountryName_Parameter()
        {
            //Arrange
            TextualSearchRequest request = new()
            {
                CountryName = "ta"
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => !x.Author.Country.Name.EndsWith("ta")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Starting_With_Name_And_Ending_With_AuthorName()
        {
            //Arrange
            BookSearchRequest request = new()
            {
                Start = 0,
                Max = 50,
                SearchData = new BookSearchDto
                {
                    Name = "Anna",
                    AuthorName = "toy"
                }
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Name.StartsWith("Anna") && x.Author.Name.EndsWith("toy")));
        }
    }
}
