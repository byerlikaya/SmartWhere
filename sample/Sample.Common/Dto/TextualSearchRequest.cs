using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;

namespace Sample.Common.Dto
{
    public class TextualSearchRequest : IWhereClause
    {
        [TextualWhereClause(StringMethod.Contains)]
        public string Name { get; set; }

        [TextualWhereClause("Name", StringMethod.NotContains)]
        public string BookName { get; set; }

        [TextualWhereClause("Author.Name", StringMethod.NotStartsWith)]
        public string AuthorName { get; set; }

        [TextualWhereClause("Author.Country.Name", StringMethod.NotEndsWith)]
        public string CountryName { get; set; }
    }
}
