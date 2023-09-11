using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WhereClauseAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public Method Method { get; set; }

        public WhereClauseOperator WhereClauseOperator { get; set; }

        public WhereClauseAttribute(WhereClauseOperator whereClauseOperator = WhereClauseOperator.And, Method method = Method.Contains)
        {
            WhereClauseOperator = whereClauseOperator;
            Method = method;
        }

        public WhereClauseAttribute(string propertyName, WhereClauseOperator whereClauseOperator = WhereClauseOperator.And, Method method = Method.Contains)
        {
            PropertyName = propertyName;
            WhereClauseOperator = whereClauseOperator;
            Method = method;
        }
    }
}
