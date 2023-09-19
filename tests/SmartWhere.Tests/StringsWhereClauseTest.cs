namespace SmartWhere.Test
{
    public class StringsWhereClauseTest
    {
        private readonly IQueryable<Book> _books = PublisherData.FillMockData()
            .SelectMany(x => x.Books)
            .AsQueryable();

        [Fact]
        public void SmartWhere_Should_Return_Books_Starting_With_Name()
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
        public void SmartWhere_Should_Return_Books_Ending_AuthorName()
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
        public void SmartWhere_Should_Return_Books_Starting_Name_Ending_AuthorName()
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
