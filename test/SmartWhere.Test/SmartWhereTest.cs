using SmartWhere.Sample.Common;
using SmartWhere.Sample.Common.Dto;
using SmartWhere.Sample.Common.Entity;

namespace SmartWhere.Test
{
    public class SmartWhereTest
    {
        private readonly IQueryable<Publisher> _publishers = PublisherData.FillDummyData();

        [Fact]
        public void SmartWhere_0()
        {
            PublisherSearchRequest request = new()
            {
                BookName = "Middlemarch"
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.Name == "Middlemarch")));
        }

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
        public void SmartWhere_2()
        {
            PublisherSearchRequest request = new()
            {
                PublishedYear = 2010
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.PublishedYear > 2010)));
        }

        [Fact]
        public void SmartWhere_3()
        {
            PublisherSearchRequest request = new()
            {
                //PublishedYear = 2010,
                AuthorName = "Name 25"
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.Author.Name == "Name 25")));
        }

        [Fact]
        public void SmartWhere_4()
        {
            PublisherSearchRequest request = new()
            {
                //PublishedYear = 2010,
                AuthorAge = 30
            };

            var result = _publishers.Where(request);

            Assert.True(result.Any(x => x.Books.Any(b => b.Author.Age == 30)));
        }
    }
}