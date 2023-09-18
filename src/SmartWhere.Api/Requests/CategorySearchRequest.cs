using SmartWhere.Attributes;
using SmartWhere.Interfaces;

namespace SmartWhere.Sample.Api.Requests
{
    public class CategorySearchRequest : IWhereClause
    {
        [WhereClause("CategoryID")]
        public int? CategoryId { get; set; }

        [WhereClause]
        public string CategoryName { get; set; }

        [WhereClause("Product.ProductName")]
        public string ProductName { get; set; }
    }
}
