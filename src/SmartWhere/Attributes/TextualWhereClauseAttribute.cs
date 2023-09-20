using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    public class TextualWhereClauseAttribute : WhereClauseAttribute
    {
        public StringMethod Method { get; set; }

        public TextualWhereClauseAttribute(StringMethod method, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(logicalOperator) => Method = method;

        public TextualWhereClauseAttribute(string propertyName, StringMethod method, LogicalOperator logicalOperator = LogicalOperator.AND)
            : base(propertyName, logicalOperator) => Method = method;
    }
}
