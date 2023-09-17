using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    public class StringsWhereClauseAttribute : WhereClauseAttribute
    {
        public StringMethod Method { get; set; }

        public StringsWhereClauseAttribute(StringMethod method, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(logicalOperator) => Method = method;

        public StringsWhereClauseAttribute(string propertyName, StringMethod method, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(propertyName, logicalOperator) => Method = method;
    }
}
