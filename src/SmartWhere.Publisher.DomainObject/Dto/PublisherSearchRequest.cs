using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;
using SmartWhere.Sample.Common.Entity;

namespace SmartWhere.Sample.Common.Dto
{
    public class PublisherSearchRequest : IWhereClause
    {
        [WhereClause(PropertyName = nameof(Publisher.Id))]
        public int? PublisherId { get; set; }

        [WhereClause]
        public string Name { get; set; }

        [WhereClause("Book.Name")]
        public string BookName { get; set; }

        [NumericsWhereCaluse("Book.PublishedYear", ComparisonOperator.GreaterThan)]
        public int? PublishedYear { get; set; }


        [NumericsWhereCaluse("Book.Author.Age", ComparisonOperator.GreaterThanOrEqual)]
        public int? AuthorAge { get; set; }

        [WhereClause("Books.Author.Name")]
        public string AuthorName { get; set; }
    }
}
