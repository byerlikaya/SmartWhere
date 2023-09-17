using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    public class NumericsWhereCaluseAttribute : WhereClauseAttribute
    {
        public ComparisonOperator ComparisonOperator { get; set; }

        public NumericsWhereCaluseAttribute(ComparisonOperator comparisonOperator, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(logicalOperator) => ComparisonOperator = comparisonOperator;

        public NumericsWhereCaluseAttribute(string propertyName, ComparisonOperator comparisonOperator, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(propertyName, logicalOperator) => ComparisonOperator = comparisonOperator;
    }
}
