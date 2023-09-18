using SmartWhere.Attributes;
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

        private static Expression GetComparison<T>(
            Expression parameter,
            PropertyInfo whereClauseProperty,
            object propertyValue,
            Expression comparison) =>
            whereClauseProperty.PropertyNameControl<T>()
                ? BasicComparison(parameter, whereClauseProperty, propertyValue, comparison)
                : ComplexComparison<T>(parameter, whereClauseProperty, propertyValue, comparison);

        private static Expression BasicComparison(
            Expression parameter,
            PropertyInfo whereClauseProperty,
            object propertyValue,
            Expression comparison)
        {
            var whereClauseAttribute = whereClauseProperty.GetWhereClauseAttribute();

            var methodExpression = MethodExpressionForBasicComparison(parameter, whereClauseAttribute, whereClauseProperty, propertyValue);

            return comparison.IsNull()
                ? methodExpression
                : whereClauseAttribute.LogicalOperator == LogicalOperator.AND
                    ? Expression.And(comparison, methodExpression!)
                    : Expression.Or(comparison, methodExpression!);
        }

        private static Expression MethodExpressionForBasicComparison(
            Expression parameter,
            WhereClauseAttribute whereClauseAttribute,
            PropertyInfo whereClauseProperty,
            object propertyValue)
        {
            var memberExpression = string.IsNullOrEmpty(whereClauseAttribute!.PropertyName)
                ? Expression.Property(parameter, whereClauseProperty.Name)
                : Expression.Property(parameter, whereClauseAttribute.PropertyName);

            var expression = SetExpressionByNull(whereClauseProperty, propertyValue, memberExpression.Type);

            var methodExpression = SetMethodExpressionByType(memberExpression, whereClauseAttribute, expression);

            return methodExpression;
        }

        private static Expression SetExpressionByNull(PropertyInfo whereClauseProperty, object propertyValue,
            Type memberExpressionType)
        {
            if (!whereClauseProperty.PropertyType.IsNullableType())
                return Expression.Constant(propertyValue);

            var constantExpression = Expression.Constant(Convert.ChangeType(propertyValue, whereClauseProperty.PropertyType.GenericTypeArguments[0]));
            return Expression.Convert(constantExpression, memberExpressionType);

        }

        private static Expression SetMethodExpressionByType(Expression memberExpression,
            WhereClauseAttribute whereClauseAttribute, Expression expression)
        {
            Expression methodExpression = null;

            if (memberExpression.Type == typeof(string))
            {
                if (whereClauseAttribute.GetType().BaseType == typeof(WhereClauseAttribute))
                    methodExpression = Expression.Call(memberExpression, ((StringsWhereClauseAttribute)whereClauseAttribute).MethodInfo(), expression);
                else
                    methodExpression = Expression.Equal(memberExpression, expression);
            }
            else if (memberExpression.Type == typeof(int))
            {
                methodExpression = Expression.Equal(memberExpression, expression);
            }

            return methodExpression;
        }

        private static Expression ComplexComparison<T>(
            Expression baseParameter,
            PropertyInfo whereClauseProperty,
            object propertyValue,
            Expression comparison)
        {
            var whereClauseAttribute = whereClauseProperty.GetWhereClauseAttribute();

            var properties = typeof(T)
                .PropertyInfos(whereClauseAttribute!.PropertyName)
                .ToList();

            MemberExpression lastMember = null;
            MemberExpression lastEnumerableMember = null;
            Type currentType = null;
            ParameterExpression parameterExpression = null;
            var index = 0;

            foreach (var (propertyInfo, propertyType) in properties)
            {
                if (!propertyType.Namespace!.StartsWith("System") || propertyInfo.PropertyType!.IsEnumarableType())
                {
                    lastMember = lastEnumerableMember = SetLastMember(
                        lastMember,
                        baseParameter,
                        propertyInfo,
                        currentType);

                    currentType = propertyInfo.PropertyType;
                }
                else
                {
                    var type = SetType(currentType, properties[index].propertyType);

                    parameterExpression = SetParameterExpression(
                        currentType,
                        type,
                        parameterExpression);

                    var memberExpression = SetMemberExpression(
                        currentType,
                        parameterExpression,
                        propertyInfo,
                        lastMember);

                    var methodExpression = MethodExpressionForComplexComparison(
                        currentType,
                        propertyType,
                        propertyValue,
                        memberExpression,
                        whereClauseAttribute,
                        whereClauseProperty,
                        lastMember,
                        parameterExpression,
                        lastEnumerableMember);

                    comparison = comparison.IsNull()
                        ? methodExpression
                        : Expression.And(comparison!, methodExpression!);

                    index++;
                }
            }

            return comparison;
        }

        private static MemberExpression SetLastMember(
            MemberExpression lastMember,
            Expression baseParameter,
            MemberInfo propertyInfo,
            Type currentType)
        {
            if (lastMember.IsNull())
            {
                lastMember = Expression.Property(baseParameter, propertyInfo.Name);
            }
            else
            {
                var type = currentType!.GetGenericArguments().FirstOrDefault();

                lastMember = currentType.IsEnumarableType()
                    ? Expression.MakeMemberAccess(Expression.Parameter(type!, type!.Name.ToLower()), propertyInfo)
                    : Expression.Property(lastMember!, propertyInfo.Name);
            }

            return lastMember;
        }

        private static Type SetType(Type currentType, Type propertyType) =>
            currentType.IsEnumarableType()
                ? currentType!.GetGenericArguments().FirstOrDefault()
                : propertyType;

        private static ParameterExpression SetParameterExpression(
            Type currentType,
            Type type,
            ParameterExpression parameterExpression)
        {
            if (currentType.IsEnumarableType())
            {
                return Expression.Parameter(type!, type!.Name.ToLower());
            }

            return parameterExpression.IsNull()
                ? Expression.Parameter(type, type.Name.ToLower())
                : parameterExpression;

        }

        private static MemberExpression SetMemberExpression(
            Type currentType,
            Expression parameterExpression,
            MemberInfo propertyInfo,
            Expression lastMember) =>
            currentType.IsEnumarableType()
                ? Expression.MakeMemberAccess(parameterExpression, propertyInfo)
                : Expression.MakeMemberAccess(lastMember, propertyInfo);

        private static Expression MethodExpressionForComplexComparison(
            Type currentType,
            Type propertyType,
            object propertyValue,
            Expression memberExpression,
            WhereClauseAttribute whereClauseAttribute,
            PropertyInfo whereClauseProperty,
            Expression lastMember,
            ParameterExpression parameterExpression,
            MemberExpression lastEnumerableMember)
        {
            Expression methodExpression = null;

            if (currentType.IsEnumarableType())
            {
                var expression = SetExpressionByNull(whereClauseProperty, propertyValue, memberExpression.Type);

                methodExpression = SetMethodExpressionByType(memberExpression, whereClauseAttribute, expression);

                methodExpression =
                    Expression.Call(typeof(Enumerable),
                        "Any",
                        new[] { lastMember.Type.GetGenericArguments().FirstOrDefault() },
                        lastMember,
                        Expression.Lambda(methodExpression, parameterExpression));
            }
            else
            {
                if (propertyType == typeof(string))
                {
                    if (lastEnumerableMember.IsNotNull() && lastEnumerableMember.Type.IsEnumarableType())
                    {
                        methodExpression =
                            Expression.Call(typeof(Enumerable),
                                "Any",
                                new[] { lastEnumerableMember.Type.GetGenericArguments().FirstOrDefault() },
                                lastEnumerableMember,
                                Expression.Lambda(methodExpression!, parameterExpression));
                    }
                    else
                    {
                        methodExpression =
                            Expression.Call(memberExpression, ((StringsWhereClauseAttribute)whereClauseAttribute).MethodInfo(), Expression.Constant(propertyValue));
                    }
                }
                else if (propertyType == typeof(int))
                {
                    methodExpression = Expression.Equal(memberExpression, Expression.Constant(propertyValue));
                }
            }

            return methodExpression;
        }
    }
}