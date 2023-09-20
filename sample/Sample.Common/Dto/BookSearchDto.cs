using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;

namespace Sample.Common.Dto;

public class BookSearchDto : IWhereClause
{
    [TextualWhereClause(StringMethod.StartsWith)]
    public string Name { get; set; }

    [TextualWhereClause("Author.Name", StringMethod.EndsWith)]
    public string AuthorName { get; set; }

}