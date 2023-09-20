using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    public class TextsWhereClauseAttribute : WhereClauseAttribute
    {
        public StringMethod Method { get; set; }

        public TextsWhereClauseAttribute(StringMethod method, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(logicalOperator) => Method = method;

        public TextsWhereClauseAttribute(string propertyName, StringMethod method, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(propertyName, logicalOperator) => Method = method;
    }
}
