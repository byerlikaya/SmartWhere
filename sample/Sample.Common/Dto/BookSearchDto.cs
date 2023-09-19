using SmartWhere.Attributes;
using SmartWhere.Enums;
using SmartWhere.Interfaces;

namespace Sample.Common.Dto;

public class BookSearchDto : IWhereClause
{
    [StringsWhereClause(StringMethod.StartsWith)]
    public string Name { get; set; }

    [StringsWhereClause("Author.Name", StringMethod.EndsWith)]
    public string AuthorName { get; set; }

}