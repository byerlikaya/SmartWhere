using SmartWhere.Enums;

namespace SmartWhere.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WhereClauseAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public WhereClauseOperator WhereClauseOperator { get; set; }

        public WhereClauseAttribute(WhereClauseOperator whereClauseOperator = WhereClauseOperator.And)
        {
            WhereClauseOperator = whereClauseOperator;
        }

        public WhereClauseAttribute(string propertyName, WhereClauseOperator whereClauseOperator = WhereClauseOperator.And)
        {
            PropertyName = propertyName;
            WhereClauseOperator = whereClauseOperator;
        }
    }
}
