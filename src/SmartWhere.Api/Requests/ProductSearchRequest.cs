using SmartWhere.Attributes;
using SmartWhere.Interfaces;

namespace SmartWhere.Sample.Api.Requests
{
    public class ProductSearchRequest : IWhereClause
    {
        //[WhereClause("ProductId")]
        public int? Id { get; set; }

        //[StringsWhereClause("ProductName", StringMethod.Contains)]
        public string Name { get; set; }

        [WhereClause]
        public decimal? UnitPrice { get; set; }

        //[WhereClause]
        public int? UnitsInStock { get; set; }

        //[WhereClause]
        public int? UnitsOnOrder { get; set; }

        //[WhereClause]
        public int? ReorderLevel { get; set; }

        //[WhereClause]
        public bool Discontinued { get; set; }
    }
}

