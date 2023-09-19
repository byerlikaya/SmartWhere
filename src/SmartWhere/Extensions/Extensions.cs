using SmartWhere.Interfaces;
using System.Reflection;

namespace SmartWhere.Extensions
{
    public static class Extensions
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

        internal static IEnumerable<(PropertyInfo propertyInfo, Type propertyType)> PropertyInfos(this Type entityType, string propertyName)
        {
            var propertiesList = new List<(PropertyInfo propertyInfo, Type propertyType)>();

            var entityProperties = new List<(PropertyInfo propertyInfo, Type propertyType)>();

            entityType.GetProperties().ToList().ForEach(x =>
            {
                entityProperties.Add(x.PropertyType.IsEnumarableType()
                    ? (x, x.PropertyType.GetGenericArguments().FirstOrDefault())
                    : (x, x.PropertyType));
            });

            var properties = propertyName.Split('.');

            var index = -1;

            foreach (var property in properties)
            {
                var entityPropertInfo = entityProperties.FirstOrDefault(x => string.Equals(x.propertyType.Name, property, StringComparison.OrdinalIgnoreCase));

                if (entityPropertInfo.propertyType.IsNotNull())
                {
                    propertiesList.Add((entityPropertInfo.propertyInfo, entityPropertInfo.propertyType));
                    index++;
                }
                else
                {
                    if (propertiesList.IsNotNullAndAny())
                    {
                        var propertyInfo = propertiesList[index].propertyType.IsEnumarableType()
                            ? propertiesList[index].propertyType.GetGenericArguments().FirstOrDefault()!.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                            : propertiesList[index].propertyType.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                        if (propertyInfo.IsNull())
                            continue;

                        propertiesList.Add((propertyInfo, propertyInfo!.PropertyType));
                        index++;
                    }
                    else
                    {
                        entityPropertInfo = entityProperties.FirstOrDefault(x => string.Equals(x.propertyInfo.Name, property, StringComparison.OrdinalIgnoreCase));

                        if (entityPropertInfo.propertyType.IsNull())
                            continue;

                        propertiesList.Add((entityPropertInfo.propertyInfo, entityPropertInfo.propertyType));
                        index++;
                    }
                }
            }

            return propertiesList;
        }

    }
}
