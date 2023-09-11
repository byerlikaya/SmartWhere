using SmartWhere.Attributes;
using SmartWhere.Interfaces;
using System.Reflection;

namespace SmartWhere.Extensions
{
    public static class Utils
    {
        internal static List<PropertyInfo> GetWhereClauseProperties(this IWhereClause whereClauseDto, out object valueData)
        {
            valueData = whereClauseDto;

            var whereClauseProperties = whereClauseDto
                .GetType()
                .GetProperties()
                .Where(x => x.GetWhereClauseAttribute().IsNotNull())
                .ToList();

            if (whereClauseProperties.IsNotNullAndAny())
                return whereClauseProperties;

            whereClauseProperties = whereClauseDto
                .GetType()
                .GetProperties()
                .Where(x => x.GetWhereClauseClassAttribute().IsNotNull())
                .ToList();

            if (whereClauseProperties.IsNullOrNotAny())
                return new List<PropertyInfo>();

            valueData = whereClauseProperties.FirstOrDefault()!.GetValue(whereClauseDto);

            whereClauseProperties = whereClauseProperties.SelectMany(x =>
                    x.PropertyType
                        .GetProperties()
                        .Where(p => p.GetWhereClauseAttribute()
                            .IsNotNull()))
                .ToList();

            return whereClauseProperties;
        }

        internal static WhereClauseAttribute GetWhereClauseAttribute(this MemberInfo memberInfo) =>
            (WhereClauseAttribute)memberInfo.GetCustomAttribute(typeof(WhereClauseAttribute), false);

        internal static MethodInfo MethodInfo(this WhereClauseAttribute whereClauseAttribute) =>
            typeof(string).GetMethod(whereClauseAttribute.Method.ToString(), new[] { typeof(string) });

        internal static bool PropertyNameControl<T>(this MemberInfo memberInfo)
        {
            var whereClauseAttribute = (WhereClauseAttribute)memberInfo.GetCustomAttribute(typeof(WhereClauseAttribute), false);

            var properties = typeof(T).GetProperties();

            return string.IsNullOrEmpty(whereClauseAttribute!.PropertyName)
                ? properties.Any(x => AreStringsEqual(x.Name, memberInfo.Name))
                : properties.Any(x => AreStringsEqual(x.Name, whereClauseAttribute!.PropertyName));
        }

        internal static bool ValueControl(this object value) =>
            value.IsNull() || string.IsNullOrEmpty(value!.ToString()) || value is int i && i.IsDefault();

        internal static bool IsNullableType(this Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        internal static bool IsEnumarableType(this Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);

        internal static IEnumerable<(PropertyInfo propertyInfo, Type propertyType)> PropertyInfos(this Type entityType, string propertyName)
        {
            //if (string.IsNullOrEmpty(propertyName))
            //    return new List<(PropertyInfo propertyInfo, Type type)>();

            var propertiesList = new List<(PropertyInfo propertyInfo, Type propertyType)>();

            var entityProperties = new List<(PropertyInfo propertyInfo, Type propertyType)>();

            entityType.GetProperties().ToList().ForEach(x =>
            {
                entityProperties.Add(x.PropertyType.IsEnumarableType()
                    ? (x, x.PropertyType.GetGenericArguments().FirstOrDefault())
                    : (x, x.PropertyType));
            });

            var properties = propertyName.Split('.');

            var index = 0;

            foreach (var property in properties)
            {
                var aa = entityProperties.FirstOrDefault(x => string.Equals(x.propertyType.Name, property, StringComparison.OrdinalIgnoreCase));

                if (aa.propertyType.IsNotNull())
                {
                    //var prop = aa!.propertyType.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                    propertiesList.Add((aa.propertyInfo, aa.propertyType));
                }
                else
                {
                    var aaa = propertiesList[index].propertyType.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                    propertiesList.Add((aaa, aaa.PropertyType));
                    index++;
                }
            }

            //for (var i = 0; i < properties.Length; i++)
            //{
            //    if (entityType.IsEnumarableType())
            //    {
            //        entityType = entityType.GetGenericArguments().FirstOrDefault();

            //        //if (entityType.IsNull())
            //        //    return new Dictionary<PropertyInfo, Type>();
            //    }

            //    if (i == properties.Length - 1)
            //    {
            //        var property = entityType!.GetProperty(properties[i]!, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            //        //if (property.IsNotNull())
            //        //    propertiesMap.Add(property);
            //    }
            //    else
            //    {
            //        var property = entityType!.GetProperty(properties[i]!, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            //        if (property.IsNull())
            //        {
            //            //entityType.GetProperties().ToList().ForEach(x =>
            //            //{
            //            //    propertiesMap.Add(x, x.PropertyType.IsEnumarableType()
            //            //        ? x.PropertyType.GetGenericArguments().FirstOrDefault()
            //            //        : x.PropertyType);
            //            //});

            //            property = entityProperties.FirstOrDefault(x => x.propertyInfo.Name == properties[i]).propertyInfo;

            //            if (property.IsNull())
            //                return new List<(PropertyInfo propertyInfo, Type propertyType)>();
            //        }

            //        entityType = property!.PropertyType;
            //        //propertiesMap.Add(property);
            //    }
            //}

            return propertiesList;
        }

        private static WhereClauseClassAttribute GetWhereClauseClassAttribute(this MemberInfo memberInfo) =>
            (WhereClauseClassAttribute)memberInfo.GetCustomAttribute(typeof(WhereClauseClassAttribute), false);

        private static bool AreStringsEqual(string firstString, string secondString) =>
            string.Equals(firstString, secondString, StringComparison.OrdinalIgnoreCase);
    }
}
