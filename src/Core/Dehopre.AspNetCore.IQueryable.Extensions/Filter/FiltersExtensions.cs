namespace Dehopre.AspNetCore.IQueryable.Extensions.Filter
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class FiltersExtensions
    {
        public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> result, ICustomQueryable model)
        {
            if (model is null)
            {
                return result;
            }

            var lastExpression = result.FilterExpression(model);
            return lastExpression is null ? result : result.Where(lastExpression);
        }

        public static Expression<Func<TEntity, bool>> FilterExpression<TEntity>(this IQueryable<TEntity> _, ICustomQueryable model)
        {
            if (model is null)
            {
                return null;
            }

            Expression lastExpression = null;
            var operations = ExpressionFactory.GetOperators<TEntity>(model);
            foreach (var expression in operations.Ordered())
            {
                if (!expression.Criteria.CaseSensitive)
                {
                    expression.FieldToFilter = Expression.Call(expression.FieldToFilter, typeof(string).GetMethods().First(m => m.Name == "ToUpper" && m.GetParameters().Length == 0));
                    expression.FiterBy = Expression.Call(expression.FiterBy, typeof(string).GetMethods().First(m => m.Name == "ToUpper" && m.GetParameters().Length == 0));
                }

                var actualExpression = GetExpression<TEntity>(expression);
                if (expression.Criteria.UseNot)
                {
                    actualExpression = Expression.Not(actualExpression);
                }

                if (lastExpression is null)
                {
                    lastExpression = actualExpression;
                }
                else
                {
                    lastExpression = expression.Criteria.UseOr ? Expression.Or(lastExpression, actualExpression) : Expression.And(lastExpression, actualExpression);
                }
            }

            return lastExpression is not null ? Expression.Lambda<Func<TEntity, bool>>(lastExpression, operations.ParameterExpression) : null;
        }

        private static Expression GetExpression<TEntity>(ExpressionParser expression)
        {
            switch (expression.Criteria.Operator)
            {
                case WhereOperator.Equals:
                    return Expression.Equal(expression.FieldToFilter, expression.FiterBy);
                case WhereOperator.NotEquals:
                    return Expression.NotEqual(expression.FieldToFilter, expression.FiterBy);
                case WhereOperator.GreaterThan:
                    return Expression.GreaterThan(expression.FieldToFilter, expression.FiterBy);
                case WhereOperator.LessThan:
                    return Expression.LessThan(expression.FieldToFilter, expression.FiterBy);
                case WhereOperator.GreaterThanOrEqualTo:
                    return Expression.GreaterThanOrEqual(expression.FieldToFilter, expression.FiterBy);
                case WhereOperator.LessThanOrEqualTo:
                    return Expression.LessThanOrEqual(expression.FieldToFilter, expression.FiterBy);
                case WhereOperator.Contains:
                    return ContainsExpression<TEntity>(expression);
                case WhereOperator.StartsWith:
                    return Expression.Call(expression.FieldToFilter, typeof(string).GetMethods().First(m => m.Name == "StartsWith" && m.GetParameters().Length == 1), expression.FiterBy);
                default:
                    return Expression.Equal(expression.FieldToFilter, expression.FiterBy);
            }
        }

        private static Expression ContainsExpression<TEntity>(ExpressionParser expression)
        {
            if (expression.Criteria.Property.IsPropertyCollection())
            {
                var methodToApplyContains = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.Name == "Contains" && x.GetParameters().Length == 2).MakeGenericMethod(expression.FieldToFilter.Type);
                return Expression.Call(methodToApplyContains, expression.FiterBy, expression.FieldToFilter);
            }
            else
            {
                var methodToApplyContains = expression.FieldToFilter.Type.GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Length == 1);
                return Expression.Call(expression.FieldToFilter, methodToApplyContains, expression.FiterBy);
            }
        }
    }
}
