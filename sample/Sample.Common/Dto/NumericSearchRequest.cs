using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;

namespace Sample.Common.Dto
{
    public class NumericSearchRequest : IWhereClause
    {
        [NumericsWhereCaluse("Author.Age", ComparisonOperator.GreaterThan)]
        public int? AuthorAge { get; set; }

        [NumericsWhereCaluse("PublishedYear", ComparisonOperator.GreaterThanOrEqual)]
        public int? PublishedStartYear { get; set; }

        [NumericsWhereCaluse("PublishedYear", ComparisonOperator.LessThanOrEqual)]
        public int? PublishedEndYear { get; set; }

        [NumericsWhereCaluse("Price", ComparisonOperator.LessThan)]
        public decimal? Price { get; set; }

        [NumericsWhereCaluse("Price", ComparisonOperator.GreaterThanOrEqual)]
        public decimal? StartPrice { get; set; }

        [NumericsWhereCaluse("Price", ComparisonOperator.LessThanOrEqual)]
        public decimal? EndPrice { get; set; }
    }
}
