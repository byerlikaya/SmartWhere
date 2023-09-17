using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    public class StringsWhereClauseAttribute : WhereClauseAttribute
    {
        public Method Method { get; set; }

        public StringsWhereClauseAttribute(WhereClauseOperator whereClauseOperator = WhereClauseOperator.And, Method method = Method.Contains) : base(whereClauseOperator)
        {
            Method = method;
        }

        public StringsWhereClauseAttribute(string propertyName, WhereClauseOperator whereClauseOperator = WhereClauseOperator.And, Method method = Method.Contains) : base(propertyName, whereClauseOperator)
        {
            Method = method;
        }
    }
}
