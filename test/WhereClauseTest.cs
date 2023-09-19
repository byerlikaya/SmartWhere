using SmartWhere.Sample.Common;
using SmartWhere.Sample.Common.Dto;
using SmartWhere.Sample.Common.Entity;

namespace SmartWhere.Test
{
    public class WhereClauseTest
    {
        private readonly IQueryable<Publisher> _publishers = PublisherData.FillDummyData();

        [Fact]
        public void SmartWhere_1()
        {
            PublisherSearchRequest request = new()
            {
                PublisherId = 3
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Id == 3));
        }

        [Fact]
        public void SmartWhere_00()
        {
            PublisherSearchRequest request = new()
            {
                Name = "Publisher 4"
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Name == "Publisher 4"));
        }

        [Fact]
        public void SmartWhere_000()
        {
            PublisherSearchRequest request = new()
            {
                PublisherId = 4,
                Name = "Publisher 4"
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Name == "Publisher 4" && x.Id == 4));
        }

        [Fact]
        public void SmartWhere_3()
        {
            PublisherSearchRequest request = new()
            {
                AuthorName = "George Eliot"
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.Author.Name == "George Eliot")));
        }

        [Fact]
        public void SmartWhere_33()
        {
            PublisherSearchRequest request = new()
            {
                BookName = "Middlemarch"
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.Name == "Middlemarch")));
        }

        [Fact]
        public void SmartWhere_0()
        {
            PublisherSearchRequest request = new()
            {
                BookName = "Middlemarch",
                AuthorName = "George Eliot"
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.Name == "Middlemarch" && b.Author.Name == "George Eliot")));
        }



        [Fact]
        public void SmartWhere_2()
        {
            PublisherSearchRequest request = new()
            {
                BookPublishedYear = 1986
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.PublishedYear == 1986)));
        }


        [Fact]
        public void SmartWhere_4()
        {
            PublisherSearchRequest request = new()
            {
                AuthorAge = 30
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.Author.Age == 30)));
        }

        [Fact]
        public void SmartWhere_5()
        {
            PublisherSearchRequest request = new()
            {
                AuthorCounty = "Turkey"
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.Author.Country.Name == "Turkey")));
        }
    }
}