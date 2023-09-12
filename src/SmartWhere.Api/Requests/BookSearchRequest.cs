using SmartWhere.Attributes;
using SmartWhere.Interfaces;

namespace SmartWhere.Sample.Api.Requests
{

    public class BookSearchRequest : IWhereClause
    {
        public int Start { get; set; }

        public int Max { get; set; }

        [WhereClauseClass]
        public BookSearchData Data { get; set; }
    }

    public class BookSearchData
    {
        [WhereClause]
        public string Name { get; set; }

        [WhereClause("Author.Name")]
        public string AuthorName { get; set; }
    }
}
