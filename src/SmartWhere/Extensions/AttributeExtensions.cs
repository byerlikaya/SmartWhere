using SmartWhere.Attributes;
using System.Reflection;

namespace SmartWhere.Extensions
{
    public static class AttributeExtensions
    {
        internal static WhereClauseAttribute GetWhereClauseAttribute(this MemberInfo memberInfo) =>
            (WhereClauseAttribute)memberInfo.GetCustomAttribute(typeof(WhereClauseAttribute), false);

        internal static WhereClauseClassAttribute GetWhereClauseClassAttribute(this MemberInfo memberInfo) =>
            (WhereClauseClassAttribute)memberInfo.GetCustomAttribute(typeof(WhereClauseClassAttribute), false);

        internal static MethodInfo MethodInfo(this TextualWhereClauseAttribute stringsWhereClauseAttribute) =>
            typeof(string).GetMethod(stringsWhereClauseAttribute.StringMethod.ToString(), new[] { typeof(string) });
    }
}
