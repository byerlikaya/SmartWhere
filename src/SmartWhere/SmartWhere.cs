using SmartWhere.Enums;
using SmartWhere.Extensions;
using SmartWhere.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartWhere
{
    public static class SmartWhere
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IWhereClause whereClauseDto)
        {
            var whereClauseProperties = whereClauseDto.GetWhereClauseProperties(out var valueData);

            if (whereClauseProperties.IsNullOrNotAny())
                return source.Where(x => true);

            Expression comparison = null;

            var parameter = Expression.Parameter(typeof(T), typeof(T).Name.ToLower());

            foreach (var whereClauseProperty in whereClauseProperties!)
            {
                var propertyValue = whereClauseProperty!.GetValue(valueData);

                if (propertyValue.ValueControl())
                    continue;

                comparison = GetComparison<T>(parameter, whereClauseProperty, propertyValue, comparison);
            }

            return comparison.IsNotNull()
                 ? source.Where(Expression.Lambda<Func<T, bool>>(comparison!, parameter))
                 : source.Where(x => true);
        }

        private static Expression GetComparison<T>(Expression parameter, PropertyInfo whereClauseProperty, object propertyValue, Expression comparison) =>
            whereClauseProperty.PropertyNameControl<T>()
                ? BaseComparison(parameter, whereClauseProperty, propertyValue, comparison)
                : ComplexComparison<T>(parameter, whereClauseProperty, propertyValue, comparison);

        private static Expression BaseComparison(Expression parameter, PropertyInfo whereClauseProperty, object propertyValue, Expression comparison)
        {
            var whereClauseAttribute = whereClauseProperty.GetWhereClauseAttribute();

            var property = string.IsNullOrEmpty(whereClauseAttribute!.PropertyName)
                ? Expression.Property(parameter, whereClauseProperty.Name)
                : Expression.Property(parameter, whereClauseAttribute.PropertyName);

            Expression expression;
            BinaryExpression binaryExpression;

            if (whereClauseProperty.PropertyType.IsNullableType())
            {
                var constantExpression = Expression.Constant(Convert.ChangeType(propertyValue, whereClauseProperty.PropertyType.GenericTypeArguments[0]));
                expression = Expression.Convert(constantExpression, property.Type);
                binaryExpression = Expression.Equal(property, expression);
            }
            else
            {
                expression = Expression.Constant(propertyValue);
                binaryExpression = Expression.Equal(property, Expression.Constant(propertyValue));
            }

            return comparison.IsNull()
                ? Expression.Equal(property, expression)
                : whereClauseAttribute.WhereClauseOperator == WhereClauseOperator.And
                    ? Expression.And(comparison, binaryExpression)
                    : Expression.Or(comparison, binaryExpression);
        }

        private static Expression ComplexComparison<T>(Expression baseParameter, MemberInfo whereClauseProperty, object propertyValue, Expression comparison)
        {
            var whereClauseAttribute = whereClauseProperty.GetWhereClauseAttribute();

            var properties = typeof(T).PropertyInfos(whereClauseAttribute!.PropertyName).ToList();

            MemberExpression lastMember = null;
            MemberExpression lastEnumerableMember = null;
            Type currentType = null;
            ParameterExpression parameterExpression = null;
            var index = 0;

            foreach (var (propertyInfo, propertyType) in properties)
            {
                Type type;

                if (!propertyType.Namespace.StartsWith("System") || propertyInfo.PropertyType!.IsEnumarableType())
                {
                    if (lastMember.IsNull())
                    {
                        lastMember = lastEnumerableMember = Expression.Property(baseParameter, propertyInfo.Name);
                    }
                    else
                    {
                        type = currentType.GetGenericArguments().FirstOrDefault();
                        if (currentType.IsEnumarableType())
                        {
                            parameterExpression = Expression.Parameter(type, type.Name.ToLower());
                            lastMember = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
                        }
                        else
                        {
                            lastMember = Expression.Property(lastMember!, propertyInfo.Name);
                        }
                    }

                    currentType = propertyInfo.PropertyType;

                }
                else
                {
                    MemberExpression memberExpression;

                    if (currentType.IsEnumarableType())
                    {
                        type = currentType.GetGenericArguments().FirstOrDefault();
                        parameterExpression = Expression.Parameter(type, type.Name.ToLower());
                        memberExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);

                        Expression methodExpression = null;

                        if (propertyType == typeof(string))
                        {
                            methodExpression = Expression.Call(memberExpression, whereClauseAttribute.MethodInfo(), Expression.Constant(propertyValue));
                        }
                        else if (propertyType == typeof(int))
                        {
                            methodExpression = Expression.Equal(memberExpression, Expression.Constant(propertyValue));
                        }

                        var methodCallExpression =
                            Expression.Call(typeof(Enumerable),
                                "Any",
                                new[] { lastMember.Type.GetGenericArguments().FirstOrDefault() },
                                lastMember,
                                Expression.Lambda(methodExpression, parameterExpression));

                        comparison = comparison.IsNull()
                            ? methodCallExpression
                            : Expression.And(comparison!, methodCallExpression!);
                    }
                    else
                    {
                        type = properties[index].propertyType;
                        parameterExpression = parameterExpression.IsNull()
                            ? Expression.Parameter(type, type.Name.ToLower())
                            : parameterExpression;

                        memberExpression = Expression.MakeMemberAccess(lastMember, propertyInfo);

                        Expression methodExpression = null;

                        if (propertyType == typeof(string))
                        {
                            methodExpression = Expression.Call(memberExpression, whereClauseAttribute.MethodInfo(), Expression.Constant(propertyValue));

                            if (lastEnumerableMember.IsNotNull() && lastEnumerableMember.Type.IsEnumarableType())
                            {
                                methodExpression =
                                    Expression.Call(typeof(Enumerable),
                                        "Any",
                                        new[] { lastEnumerableMember.Type.GetGenericArguments().FirstOrDefault() },
                                        lastEnumerableMember,
                                        Expression.Lambda(methodExpression, parameterExpression));
                            }
                        }
                        else if (propertyType == typeof(int))
                        {
                            methodExpression = Expression.Equal(memberExpression, Expression.Constant(propertyValue));
                        }

                        comparison = comparison.IsNull()
                            ? methodExpression
                            : Expression.And(comparison!, methodExpression!);
                    }

                    index++;
                }
            }

            return comparison;
        }

    }
}