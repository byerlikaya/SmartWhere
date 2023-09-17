using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    public class StringsWhereClauseAttribute : WhereClauseAttribute
    {
        public Method Method { get; set; }

        public StringsWhereClauseAttribute(Method method, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(logicalOperator) => Method = method;

        public StringsWhereClauseAttribute(string propertyName, Method method, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(propertyName, logicalOperator) => Method = method;
    }
}
