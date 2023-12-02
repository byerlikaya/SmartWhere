namespace SmartWhere.Attributes;

public class TextualWhereClauseAttribute : WhereClauseAttribute
{
    public StringMethod StringMethod { get; set; }

    public TextualWhereClauseAttribute(StringMethod method, LogicalOperator logicalOperator = LogicalOperator.AND)
        : base(logicalOperator) => StringMethod = method;

    public TextualWhereClauseAttribute(string propertyName, StringMethod method, LogicalOperator logicalOperator = LogicalOperator.AND)
        : base(propertyName, logicalOperator) => StringMethod = method;
}