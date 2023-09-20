using SmartWhere.Attributes;
using System.Reflection;

namespace SmartWhere.Extensions
{
    public static class LogicalExtensions
    {
        internal static bool IsNull<T>(this T item) where T : class => item is null;

        internal static bool IsNotNull<T>(this T item) where T : class => item is not null;

        internal static bool IsNullOrNotAny<T>(this IEnumerable<T> items) => items is null || !items.Any();

        internal static bool IsNotNullAndAny<T>(this IEnumerable<T> items) => items is not null && items.Any();

        internal static bool IsNullableType(this Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        internal static bool IsEnumarableType(this Type type) =>
            type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetGenericTypeDefinition() == typeof(List<>));

        internal static bool PropertyNameControl<T>(this MemberInfo memberInfo)
        {
            var whereClauseAttribute = (WhereClauseAttribute)memberInfo.GetCustomAttribute(typeof(WhereClauseAttribute), false);

            var properties = typeof(T).GetProperties();

            return string.IsNullOrEmpty(whereClauseAttribute!.PropertyName)
                ? properties.Any(x => AreStringsEqual(x.Name, memberInfo.Name))
                : properties.Any(x => AreStringsEqual(x.Name, whereClauseAttribute!.PropertyName));
        }

        internal static bool ValueControl(this object value)
            => value.IsNull() || string.IsNullOrEmpty(value!.ToString());

        private static bool AreStringsEqual(string firstString, string secondString)
            => string.Equals(firstString, secondString, StringComparison.OrdinalIgnoreCase);
    }
}
