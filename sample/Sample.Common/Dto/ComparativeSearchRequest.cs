using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;

namespace Sample.Common.Dto
{
    public class ComparativeSearchRequest : IWhereClause
    {
        [ComparativeWhereCaluse("Author.Age", ComparisonOperator.GreaterThan)]
        public int? AuthorAge { get; set; }

        [ComparativeWhereCaluse("PublishedYear", ComparisonOperator.GreaterThanOrEqual)]
        public int? PublishedStartYear { get; set; }

        [ComparativeWhereCaluse("PublishedYear", ComparisonOperator.LessThanOrEqual)]
        public int? PublishedEndYear { get; set; }

        [ComparativeWhereCaluse("Price", ComparisonOperator.LessThan)]
        public decimal? Price { get; set; }

        [ComparativeWhereCaluse("Price", ComparisonOperator.GreaterThanOrEqual)]
        public decimal? StartPrice { get; set; }

        [ComparativeWhereCaluse("Price", ComparisonOperator.LessThanOrEqual)]
        public decimal? EndPrice { get; set; }

        [ComparativeWhereCaluse("CreatedDate", ComparisonOperator.LessThan)]
        public DateTime? BookCreatedDate { get; set; }

        [ComparativeWhereCaluse("CreatedDate", ComparisonOperator.GreaterThanOrEqual)]
        public DateTime? StartCreatedDate { get; set; }

        [ComparativeWhereCaluse("CreatedDate", ComparisonOperator.LessThanOrEqual)]
        public DateTime? EndCreatedDate { get; set; }
    }
}
