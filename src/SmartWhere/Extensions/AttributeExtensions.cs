using SmartWhere.Attributes;
using SmartWhere.Enums;
using System.Reflection;

namespace SmartWhere.Extensions
{
    public static class AttributeExtensions
    {
        internal static WhereClauseAttribute GetWhereClauseAttribute(this MemberInfo memberInfo) =>
            (WhereClauseAttribute)memberInfo.GetCustomAttribute(typeof(WhereClauseAttribute), false);

        internal static WhereClauseClassAttribute GetWhereClauseClassAttribute(this MemberInfo memberInfo) =>
            (WhereClauseClassAttribute)memberInfo.GetCustomAttribute(typeof(WhereClauseClassAttribute), false);

        internal static MethodInfo MethodInfo(this TextualWhereClauseAttribute textualWhereClause)
        {
            var methodName = textualWhereClause.StringMethod switch
            {
                StringMethod.Contains => textualWhereClause.StringMethod.ToString(),
                StringMethod.StartsWith => textualWhereClause.StringMethod.ToString(),
                StringMethod.EndsWith => textualWhereClause.StringMethod.ToString(),
                StringMethod.NotContains => nameof(StringMethod.Contains),
                StringMethod.NotStartsWith => nameof(StringMethod.StartsWith),
                StringMethod.NotEndsWith => nameof(StringMethod.EndsWith),
                _ => nameof(StringMethod.Contains)
            };

            return typeof(string).GetMethod(methodName, new[] { typeof(string)
    });
        }
    }
}
