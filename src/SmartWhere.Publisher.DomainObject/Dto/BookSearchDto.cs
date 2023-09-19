using SmartWhere.Attributes;
using SmartWhere.Interfaces;

namespace SmartWhere.Sample.Common.Dto;

public class BookSearchDto : IWhereClause
{
    [WhereClause]
    public string Name { get; set; }

    [WhereClause("Author.Name")]
    public string AuthorName { get; set; }
}