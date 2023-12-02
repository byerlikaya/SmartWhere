namespace SmartWhere.Attributes;

public class ComparativeWhereCaluseAttribute : WhereClauseAttribute
{
    public ComparisonOperator ComparisonOperator { get; set; }

    public ComparativeWhereCaluseAttribute(ComparisonOperator comparisonOperator, LogicalOperator logicalOperator = LogicalOperator.AND)
        : base(logicalOperator) => ComparisonOperator = comparisonOperator;

    public ComparativeWhereCaluseAttribute(string propertyName, ComparisonOperator comparisonOperator, LogicalOperator logicalOperator = LogicalOperator.AND)
        : base(propertyName, logicalOperator) => ComparisonOperator = comparisonOperator;
}