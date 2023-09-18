using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;

namespace SmartWhere.Sample.Api.Requests
{
    public class ProductSearchRequest : IWhereClause
    {
        [NumericsWhereCaluse("ProductID", ComparisonOperator.Equal)]
        public int? Id { get; set; }

        [WhereClause("Category.CategoryID")]
        public int? CategoryId { get; set; }

        [StringsWhereClause("ProductName", StringMethod.Contains)]
        public string Name { get; set; }

        [NumericsWhereCaluse(ComparisonOperator.GreaterThan)]
        public decimal? UnitPrice { get; set; }

        [NumericsWhereCaluse(ComparisonOperator.GreaterThanOrEqual)]
        public int? UnitsInStock { get; set; }

        [WhereClause]
        public int? UnitsOnOrder { get; set; }

        [WhereClause]
        public int? ReorderLevel { get; set; }

        [WhereClause]
        public bool? Discontinued { get; set; }
    }
}

