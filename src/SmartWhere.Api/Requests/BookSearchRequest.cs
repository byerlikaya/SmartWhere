using SmartWhere.Attributes;
using SmartWhere.Interfaces;

namespace SmartWhere.Api.Requests
{

    public class BookSearchRequest : IWhereClause
    {
        public int Start { get; set; }

        public int Max { get; set; } = 10;

        [WhereClauseClass]
        public BookSearchData Data { get; set; }
    }

    public class BookSearchData
    {
        [WhereClause]
        public string Name { get; set; }

        [WhereClause("Author.Name")]
        public string AuthorName { get; set; }

        [WhereClause("Author.Countries.Name")]
        //or [WhereClause("Author.Country.Name")]
        public string CountryName { get; set; }
    }
}
