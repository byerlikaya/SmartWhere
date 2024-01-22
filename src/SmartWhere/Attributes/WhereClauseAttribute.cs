namespace SmartWhere.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class WhereClauseAttribute(string propertyName, LogicalOperator logicalOperator = LogicalOperator.AND)
    : Attribute
{
    public string PropertyName { get; set; } = propertyName;

    public LogicalOperator LogicalOperator { get; set; } = logicalOperator;

    public WhereClauseAttribute(LogicalOperator logicalOperator = LogicalOperator.AND) : this(null, logicalOperator)
    {
    }
}