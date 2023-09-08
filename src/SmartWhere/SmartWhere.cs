using SmartWhere.Enums;
using SmartWhere.Extensions;
using SmartWhere.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartWhere
{
    public static class SmartWhere
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IWhereClause whereClauseDto) where T : class
        {
            var whereClauseProperties = whereClauseDto
                .GetType()
                .GetProperties()
                .Where(x => x.GetWhereClauseAttribute().IsNotNull())
                .ToList();

            if (whereClauseProperties.IsNullOrNotAny())
                return source.Where(x => true);

            Expression comparison = null;

            var parameter = Expression.Parameter(typeof(T), typeof(T).Name.ToLower());

            foreach (var whereClauseProperty in whereClauseProperties!)
            {
                var propertyValue = whereClauseProperty!.GetValue(whereClauseDto);

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

        private static Expression ComplexComparison<T>(Expression parameter, MemberInfo whereClauseProperty, object propertyValue, Expression comparison)
        {
            var whereClauseAttribute = whereClauseProperty.GetWhereClauseAttribute();

            var properties = typeof(T).PropertyInfos(whereClauseAttribute!.PropertyName).ToList();

            List<(Expression parameter, MemberExpression memberExpression)> members = new();

            var memberIndex = -1;
            var index = 0;

            ParameterExpression baseParameterExpression = null;
            var baseType = properties[0].propertyType;

            foreach (var (propertyInfo, propertyType) in properties)
            {
                if (propertyInfo.PropertyType!.IsEnumarableType())
                {
                    members.Add(members.IsNullOrNotAny()
                        ? (parameter, Expression.Property(parameter, propertyInfo.Name))
                        : (parameter, Expression.Property(members[memberIndex].memberExpression!, propertyInfo.Name)));
                    memberIndex++;
                }
                else
                {
                    var type = properties[index].propertyType;

                    var parameterExpression = Expression.Parameter(type, type.Name.ToLower());

                    var memberExpression = Expression.MakeMemberAccess(
                           members.Count.IsDefault(defaultValue: 1)
                               ? parameterExpression
                               : members[memberIndex].memberExpression,
                        propertyInfo);

                    Expression methodExpression = null;

                    if (propertyType == typeof(string))
                    {
                        methodExpression = Expression.Call(memberExpression, whereClauseAttribute.MethodInfo(), Expression.Constant(propertyValue));
                        members.Add((parameterExpression, memberExpression));
                        memberIndex++;
                    }
                    else if (propertyType == typeof(int))
                    {
                        methodExpression = Expression.Equal(memberExpression, Expression.Constant(propertyValue));
                        members.Add((parameterExpression, memberExpression));
                        memberIndex++;
                    }
                    else if (propertyType!.IsClass)
                    {
                        if (baseParameterExpression.IsNull())
                            baseParameterExpression = parameterExpression;
                        members.Add((parameterExpression, memberExpression));
                        memberIndex++;
                        index++;
                        continue;
                    }

                    var methodCallExpression = Expression.Call(typeof(Enumerable),
                        "Any",
                        new[] { baseType },
                        members.FirstOrDefault().memberExpression,
                        Expression.Lambda(methodExpression!, (baseParameterExpression.IsNull()
                            ? (ParameterExpression)members[memberIndex].parameter
                            : baseParameterExpression)!));

                    comparison = comparison.IsNull()
                        ? methodCallExpression
                        : Expression.And(comparison!, methodCallExpression!);

                    index++;
                }
            }

            return comparison;
        }

    }
}