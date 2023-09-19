namespace SmartWhere.Test
{
    public class NumericsWhereClauseTest
    {
        private readonly IQueryable<Book> _books = PublisherData.FillMockData()
            .SelectMany(x => x.Books)
            .AsQueryable();

        [Fact]
        public void SmartWhere_Should_Return_Books_GraterThan_AuthorAge()
        {
            //Arrange
            NumericSearchRequest request = new()
            {
                AuthorAge = 20
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Author.Age > 20));
        }

        [Fact]
        public void SmartWhere_Should_Return_Books_GraterThanOrEqual_PublishedYear()
        {
            //Arrange
            NumericSearchRequest request = new()
            {
                PublishedStartYear = 1990
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.PublishedYear >= 1990));
        }

        [Fact]
        public void SmartWhere_Should_Return_Books_LessThanOrEqual_PublishedYear()
        {
            //Arrange
            NumericSearchRequest request = new()
            {
                PublishedEndYear = 2000
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.PublishedYear <= 2000));
        }

        [Fact]
        public void SmartWhere_Should_Return_Books_GraterThanOrEqual_And_LessThanOrEqual_PublishedYear()
        {
            //Arrange
            NumericSearchRequest request = new()
            {
                PublishedStartYear = 1980,
                PublishedEndYear = 1999
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.PublishedYear >= 1980 && x.PublishedYear <= 1999));
        }

        [Fact]
        public void SmartWhere_Should_Return_Books_LessThan_Price()
        {
            //Arrange
            NumericSearchRequest request = new()
            {
                Price = 120
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Price < 120));
        }

        [Fact]
        public void SmartWhere_Should_Return_Books_GreaterThanOrEqual_Price()
        {
            //Arrange
            NumericSearchRequest request = new()
            {
                StartPrice = 120
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Price >= 120));
        }

        [Fact]
        public void SmartWhere_Should_Return_Books_LessThanOrEqual_Price()
        {
            //Arrange
            NumericSearchRequest request = new()
            {
                EndPrice = 230
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Price <= 230));
        }

        [Fact]
        public void SmartWhere_Should_Return_Books_GreaterThanOrEqual_And_LessThanOrEqual_Price()
        {
            //Arrange
            NumericSearchRequest request = new()
            {
                StartPrice = 140,
                EndPrice = 300
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Price >= 140 && x.Price <= 300));
        }
    }
}
