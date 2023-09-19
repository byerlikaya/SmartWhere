using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;
using SmartWhere.Northwind.DomainObject;

namespace SmartWhere.Sample.Api.Requests
{
    public class ProductSearchRequest : IWhereClause
    {
        [NumericsWhereCaluse(nameof(Product.ProductID), ComparisonOperator.Equal)]
        public int? Id { get; set; }

        [WhereClause($"{nameof(Category)}.{nameof(Category.CategoryID)}")]
        public int? CategoryId { get; set; }

        [StringsWhereClause(nameof(Product.ProductName), StringMethod.Contains)]
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

