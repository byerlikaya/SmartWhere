using SmartWhere.Enums;
using System;

namespace SmartWhere.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WhereClauseAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public LogicalOperator LogicalOperator { get; set; }

        public WhereClauseAttribute(LogicalOperator logicalOperator = LogicalOperator.AND) => LogicalOperator = logicalOperator;

        public WhereClauseAttribute(string propertyName, LogicalOperator logicalOperator = LogicalOperator.AND)
        {
            PropertyName = propertyName;
            LogicalOperator = logicalOperator;
        }
    }
}
