namespace SmartWhere.Extensions;

internal static class Extensions
{
    internal static List<PropertyInfo> GetWhereClauseProperties(
        this IWhereClause whereClauseDto,
        out object valueData)
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

    internal static IEnumerable<(PropertyInfo propertyInfo, Type propertyType)> PropertyInfos(
        this Type entityType,
        string propertyName)
    {
        var propertiesList = new List<(PropertyInfo propertyInfo, Type propertyType)>();

        var entityProperties = GetEntityProperties(entityType);

        var properties = propertyName.Split('.');

        var index = -1;
        var findedMainPropertyInEntity = false;

        foreach (var property in properties)
        {
            var entityPropertInfo = entityProperties.GetPropertyInfoByType(property);

            if (entityPropertInfo.propertyType.IsNotNull() && !findedMainPropertyInEntity)
            {
                propertiesList.Add((entityPropertInfo.propertyInfo, entityPropertInfo.propertyType));
                findedMainPropertyInEntity = true;
                index++;
            }
            else
            {
                if (propertiesList.IsNotNullAndAny())
                {
                    var propertyInfo = GetPropertyInfo(property, propertiesList[index]);

                    if (propertyInfo.IsNull())
                        continue;

                    propertiesList.Add((propertyInfo, propertyInfo!.PropertyType));
                    index++;
                }
                else
                {
                    entityPropertInfo = entityProperties.GetPropertyInfoByInfo(property);

                    if (entityPropertInfo.propertyType.IsNull())
                        continue;

                    propertiesList.Add((entityPropertInfo.propertyInfo, entityPropertInfo.propertyType));
                    index++;
                }
            }
        }

        return propertiesList;
    }

    private static PropertyInfo GetPropertyInfo(
        string property,
        (PropertyInfo propertyInfo, Type propertyType) properties)
    {
        PropertyInfo propertyInfo;

        if (properties.propertyType.IsEnumarableType())
        {
            propertyInfo = properties.propertyType
                .GetGenericArguments()
                .FirstOrDefault()!
                .GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        }
        else
        {
            propertyInfo = properties.propertyType
                .GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (propertyInfo.IsNull())
                properties.propertyType
                    .GetProperties()
                    .ToList()
                    .ForEach(prop =>
                    {
                        if (prop.PropertyType.IsEnumarableType())
                        {
                            var type = prop.PropertyType.GetGenericArguments().FirstOrDefault();
                            if (string.Equals(type!.Name, property, StringComparison.OrdinalIgnoreCase))
                            {
                                propertyInfo = prop;
                            }
                        }
                        else
                        {
                            if (string.Equals(prop.PropertyType.Name, property, StringComparison.OrdinalIgnoreCase))
                            {
                                propertyInfo = prop;
                            }
                        }
                    });
        }

        return propertyInfo;
    }

    private static List<(PropertyInfo propertyInfo, Type propertyType)> GetEntityProperties(Type entityType)
    {
        var entityProperties = new List<(PropertyInfo propertyInfo, Type propertyType)>();

        entityType.GetProperties().ToList().ForEach(x =>
        {
            entityProperties.Add(x.PropertyType.IsEnumarableType()
                ? (x, x.PropertyType.GetGenericArguments().FirstOrDefault())
                : (x, x.PropertyType));
        });

        return entityProperties;
    }

    private static (PropertyInfo propertyInfo, Type propertyType) GetPropertyInfoByType(
        this IEnumerable<(PropertyInfo propertyInfo, Type propertyType)> entityProperties,
        string property) =>
        entityProperties.FirstOrDefault(x => string.Equals(x.propertyType.Name, property, StringComparison.OrdinalIgnoreCase));

    private static (PropertyInfo propertyInfo, Type propertyType) GetPropertyInfoByInfo(
        this IEnumerable<(PropertyInfo propertyInfo, Type propertyType)> entityProperties,
        string property) =>
        entityProperties.FirstOrDefault(x => string.Equals(x.propertyInfo.Name, property, StringComparison.OrdinalIgnoreCase));
}