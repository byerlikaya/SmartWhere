namespace SmartWhere.Test
{
    public class WhereClauseTest
    {
        private readonly IQueryable<Publisher> _publishers = PublisherData.FillDummyData();

        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_PublisherId()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                PublisherId = 3
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Id == 3));
        }

        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_PublisherName()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                Name = "Publisher 4"
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Name == "Publisher 4"));
        }

        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_PublisherId_And_Name()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                PublisherId = 4,
                Name = "Publisher 4"
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Name == "Publisher 4" && x.Id == 4));
        }

        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_AuthorName()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                AuthorName = "George Eliot"
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Books.Any(b => b.Author.Name == "George Eliot")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_BookName()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                BookName = "Middlemarch"
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Books.Any(b => b.Name == "Middlemarch")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_BookName_And_AuthorName()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                BookName = "Middlemarch",
                AuthorName = "George Eliot"
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Books.Any(b => b.Name == "Middlemarch" && b.Author.Name == "George Eliot")));
        }

        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_BookPublishedYear()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                BookPublishedYear = 1986
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Books.Any(b => b.PublishedYear == 1986)));
        }


        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_AuthorAge()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                AuthorAge = 30
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Books.Any(b => b.Author.Age == 30)));
        }

        [Fact]
        public void SmartWhere_Should_Return_Publishers_By_AuthorCountry()
        {
            //Arrange
            PublisherSearchRequest request = new()
            {
                AuthorCountry = "Turkey"
            };

            //Act
            var result = _publishers.Where(request);

            //Assert
            Assert.True(result.Any(x => x.Books.Any(b => b.Author.Country.Name == "Turkey")));
        }
    }
}