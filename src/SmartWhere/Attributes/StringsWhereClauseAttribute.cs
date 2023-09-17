using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    public class StringsWhereClauseAttribute : WhereClauseAttribute
    {
        public Method Method { get; set; }

        public StringsWhereClauseAttribute(Method method, WhereClauseOperator whereClauseOperator = WhereClauseOperator.And)
            : base(whereClauseOperator) => Method = method;

        public StringsWhereClauseAttribute(string propertyName, Method method, WhereClauseOperator whereClauseOperator = WhereClauseOperator.And)
            : base(propertyName, whereClauseOperator) => Method = method;
    }
}
