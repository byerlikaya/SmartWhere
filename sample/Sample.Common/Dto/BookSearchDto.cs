using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;

namespace Sample.Common.Dto;

public class BookSearchDto : IWhereClause
{
    [TextsWhereClause(StringMethod.StartsWith)]
    public string Name { get; set; }

    [TextsWhereClause("Author.Name", StringMethod.EndsWith)]
    public string AuthorName { get; set; }

}