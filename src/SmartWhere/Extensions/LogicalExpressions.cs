namespace SmartWhere.Extensions
{
    public static class LogicalExpressions
    {
        public static bool IsNull<T>(this T item) where T : class => item is null;

        public static bool IsNotNull<T>(this T item) where T : class => item is not null;

        public static bool IsNullOrNotAny<T>(this IEnumerable<T> items) => items is null || !items.Any();

        public static bool IsNotNullAndAny<T>(this IEnumerable<T> items) => items is not null && items.Any();

        public static bool IsDefault(this int item) => item is default(int);
    }
}
