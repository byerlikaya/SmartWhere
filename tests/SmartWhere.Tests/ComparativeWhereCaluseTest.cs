namespace SmartWhere.Test
{
    public class ComparativeWhereCaluseTest
    {
        private readonly IQueryable<Book> _books = DataInitializer.FillMockData()
            .SelectMany(x => x.Books)
            .AsQueryable();

        [Fact]
        public void SmartWhere_Should_Return_Results_Grater_Than_AuthorAge_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                AuthorAge = 20
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Author.Age > 20));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Grater_Than_Or_Equal_PublishedStartYear_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                PublishedStartYear = 1990
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.PublishedYear >= 1990));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Less_Than_Or_Equal_PublishedEndYear_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                PublishedEndYear = 2000
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.PublishedYear <= 2000));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Grater_Than_Or_Equal_PublishedStartYear_Parameter_And_Less_Than_Or_Equal_PublishedEndYear_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
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
        public void SmartWhere_Should_Return_Results_Less_Than_Price_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                Price = 120
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Price < 120));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Greater_Than_Or_Equal_StartPrice_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                StartPrice = 120
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Price >= 120));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Less_Than_Or_Equal_EndPrice_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                EndPrice = 230
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Price <= 230));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Greater_Than_Or_Equal_StartPrice_Parameter_And_Less_Than_Or_Equal_EndPrice_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                StartPrice = 140,
                EndPrice = 300
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Price >= 140 && x.Price <= 300));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Less_Than_BookCreatedDate_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                BookCreatedDate = DateTime.Now.AddDays(-3)
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.CreatedDate <= DateTime.Now.AddDays(-3)));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Greater_Than_Or_Equal_StartCreatedDate_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                StartCreatedDate = DateTime.Now.AddDays(-15)
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.CreatedDate >= DateTime.Now.AddDays(-15)));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Less_Than_Or_Equal_EndCreatedDate_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                EndCreatedDate = DateTime.Now.AddDays(-5)
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.CreatedDate <= DateTime.Now.AddDays(-5)));
        }

        [Fact]
        public void SmartWhere_Should_Return_Results_Greater_Than_Or_Equal_StartCreatedDate_Parameter_And_Less_Than_Or_Equal_EndCreatedDate_Parameter()
        {
            //Arrange
            ComparativeSearchRequest request = new()
            {
                StartCreatedDate = DateTime.Now.AddDays(-8),
                EndCreatedDate = DateTime.Now.AddDays(-2)
            };

            //Act
            var result = _books.Where(request);

            //Assert
            Assert.True(result.Any(x => x.CreatedDate >= DateTime.Now.AddDays(-8) && x.CreatedDate <= DateTime.Now.AddDays(-2)));
        }
    }
}
