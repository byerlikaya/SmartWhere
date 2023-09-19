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
        private static readonly List<Type> Types = new()
        {
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(short?),
            typeof(int?),
            typeof(long?),
            typeof(decimal),
            typeof(decimal?),
            typeof(double),
            typeof(double?),
            typeof(bool),
            typeof(bool?),
        };

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

            return SetMethodExpressionByType(memberExpression, whereClauseAttribute, expression);
        }

        private static Expression SetExpressionByNull(
            PropertyInfo whereClauseProperty,
            object propertyValue,
            Type memberExpressionType)
        {
            if (!whereClauseProperty.PropertyType.IsNullableType())
                return Expression.Constant(propertyValue);

            var constantExpression = Expression.Constant(Convert.ChangeType(propertyValue, whereClauseProperty.PropertyType.GenericTypeArguments[0]));
            return Expression.Convert(constantExpression, memberExpressionType);

        }

        private static Expression SetMethodExpressionByType(
            Expression memberExpression,
            WhereClauseAttribute whereClauseAttribute,
            Expression expression)
        {

            if (memberExpression.Type == typeof(string))
            {
                if (whereClauseAttribute.GetType().BaseType == typeof(WhereClauseAttribute))
                    return Expression.Call(memberExpression, ((StringsWhereClauseAttribute)whereClauseAttribute).MethodInfo(), expression);

                return Expression.Equal(memberExpression, expression);
            }

            if (!Types.Contains(memberExpression.Type))
                return Expression.Equal(memberExpression, expression);

            if (whereClauseAttribute.GetType().BaseType == typeof(WhereClauseAttribute))
            {
                return ((NumericsWhereCaluseAttribute)whereClauseAttribute).ComparisonOperator switch
                {
                    ComparisonOperator.Equal => Expression.Equal(memberExpression, expression),
                    ComparisonOperator.NotEqual => Expression.NotEqual(memberExpression, expression),
                    ComparisonOperator.GreaterThan => Expression.GreaterThan(memberExpression, expression),
                    ComparisonOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(memberExpression, expression),
                    ComparisonOperator.LessThan => Expression.LessThan(memberExpression, expression),
                    ComparisonOperator.LessThanOrEqual => Expression.LessThanOrEqual(memberExpression, expression),
                    _ => Expression.Equal(memberExpression, expression)
                };
            }

            return Expression.Equal(memberExpression, expression);

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
                    currentType = currentType.IsNull()
                        ? propertyInfo.PropertyType
                        : currentType;

                    lastEnumerableMember = SetLastEnumerableMember(lastEnumerableMember, lastMember, baseParameter, propertyInfo.Name);

                    var type = SetType(currentType, properties[index].propertyType);

                    parameterExpression = SetParameterExpression(
                        currentType,
                        type,
                        parameterExpression);

                    lastMember = SetLastMember(
                        lastMember,
                        baseParameter,
                        propertyInfo,
                        currentType,
                        parameterExpression);

                    currentType = propertyInfo.PropertyType;
                }
                else
                {
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

        private static MemberExpression SetLastEnumerableMember(
            MemberExpression lastEnumerableMember,
            MemberExpression lastMember,
            Expression baseParameter,
            string propertyInfoName) =>
            lastMember.IsNull()
                ? Expression.Property(baseParameter, propertyInfoName)
                : lastEnumerableMember;

        private static MemberExpression SetLastMember(
            MemberExpression lastMember,
            Expression baseParameter,
            MemberInfo propertyInfo,
            Type currentType,
            Expression parameterExpression)
        {
            if (lastMember.IsNull())
            {
                return Expression.Property(baseParameter, propertyInfo.Name);
            }

            return currentType.IsEnumarableType()
                ? Expression.MakeMemberAccess(parameterExpression, propertyInfo)
                : Expression.Property(lastMember!, propertyInfo.Name);
        }

        private static Type SetType(
            Type currentType,
            Type propertyType) =>
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
            var expression = SetExpressionByNull(whereClauseProperty, propertyValue, memberExpression.Type);

            var methodExpression = SetMethodExpressionByType(memberExpression, whereClauseAttribute, expression);

            if (currentType.IsEnumarableType())
            {
                return Expression.Call(typeof(Enumerable),
                        "Any",
                        new[] { lastMember.Type.GetGenericArguments().FirstOrDefault() },
                        lastMember,
                        Expression.Lambda(methodExpression, parameterExpression));
            }

            if (propertyType == typeof(string))
            {
                if (lastEnumerableMember.IsNotNull() && lastEnumerableMember.Type.IsEnumarableType())
                {
                    return Expression.Call(typeof(Enumerable),
                            "Any",
                            new[] { lastEnumerableMember.Type.GetGenericArguments().FirstOrDefault() },
                            lastEnumerableMember,
                            Expression.Lambda(methodExpression!, parameterExpression));
                }

                return SetMethodExpressionByType(memberExpression, whereClauseAttribute, Expression.Constant(propertyValue));
            }

            if (Types.Contains(propertyType))
            {
                return Expression.Equal(memberExpression, Expression.Constant(propertyValue));
            }

            return Expression.Equal(memberExpression, Expression.Constant(propertyValue));
        }
    }
}