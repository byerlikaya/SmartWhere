using SmartWhere.Attributes;
using SmartWhere.Interfaces;

namespace Sample.Common.Dto
{
    public class BookSearchRequest : IWhereClause
    {
        public int Start { get; set; }

        public int Max { get; set; }


        [WhereClauseClass]
        public BookSearchDto SearchData { get; set; }
    }
}
