namespace Dehopre.AspNetCore.IQueryable.Extensions.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Dehopre.AspNetCore.IQueryable.Extensions.Attributes;

    internal static class ExpressionFactory
    {
        internal static ExpressionParserCollection GetOperators<TEntity>(ICustomQueryable model)
        {
            var expressions = new ExpressionParserCollection();
            var type = model.GetType();
            expressions.ParameterExpression = Expression.Parameter(typeof(TEntity), "model");
            foreach (var propertyInfo in type.GetProperties())
            {
                var criteria = GetCriteria(model, propertyInfo);
                if (criteria is null)
                {
                    continue;
                }

                if (!typeof(TEntity).HasProperty(criteria.FieldName) && !criteria.FieldName.Contains("."))
                {
                    continue;
                }

                dynamic propertyValue = expressions.ParameterExpression;
                foreach (var part in criteria.FieldName.Split('.'))
                {
                    propertyValue = Expression.PropertyOrField(propertyValue, part);
                }

                var expressionData = new ExpressionParser
                {
                    FieldToFilter = propertyValue,
                    FiterBy = GetClosureOverContains(criteria.Property.GetValue(model, null), GetNonNullable(criteria.Property.PropertyType)),
                    Criteria = criteria
                };
                if (criteria.Property.GetValue(model, null) is not null)
                {
                    expressions.Add(expressionData);
                }
            }

            return expressions;
        }

        internal static WhereClause GetCriteria(ICustomQueryable model, PropertyInfo propertyInfo)
        {
            var isCollection = propertyInfo.IsPropertyCollection();
            if (!isCollection && propertyInfo.IsPropertObject(model))
            {
                return null;
            }

            var criteria = new WhereClause();
            var attr = Attribute.GetCustomAttributes(propertyInfo);
            if (attr.Any(a => a.GetType() == typeof(QueryOperatorAttribute)))
            {
                var data = (QueryOperatorAttribute)attr.First(a => a.GetType() == typeof(QueryOperatorAttribute));
                criteria.UpdateAttributeData(data);
                if (data.Operator != WhereOperator.Contains && isCollection)
                {
                    throw new ArgumentException($"{propertyInfo.Name} - For array the only operator available is \"Contains\".");
                }
            }

            if (isCollection)
            {
                criteria.Operator = WhereOperator.Contains;
            }

            var customValue = propertyInfo.GetValue(model, null);
            if (customValue is null)
            {
                return null;
            }

            criteria.UpdateValues(propertyInfo);
            return criteria;
        }

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        internal static Expression GetClosureOverContains<T>(T constant, Type targetType) => Expression.Constant(constant, targetType);

        internal static List<WhereClause> GetCriterias(ICustomQueryable searchModel)
        {
            var type = searchModel.GetType();
            var criterias = new List<WhereClause>();
            // Iterate through all methods of the class.
            foreach (var propertyInfo in type.GetProperties())
            {
                var isCollection = propertyInfo.IsPropertyCollection();
                if (!isCollection && propertyInfo.IsPropertObject(searchModel))
                {
                    continue;
                }

                var criteria = new WhereClause();
                var attr = Attribute.GetCustomAttributes(propertyInfo).FirstOrDefault();
                if (attr?.GetType() == typeof(QueryOperatorAttribute))
                {
                    var data = (QueryOperatorAttribute)attr;
                    if (data.Operator != WhereOperator.Contains && isCollection)
                    {
                        throw new ArgumentException($"{propertyInfo.Name} - For array the only operator available is \"Contains\".");
                    }
                }

                if (isCollection)
                {
                    criteria.Operator = WhereOperator.Contains;
                }

                var customValue = propertyInfo.GetValue(searchModel, null);
                if (customValue is null)
                {
                    continue;
                }

                criteria.UpdateValues(propertyInfo);
                criterias.Add(criteria);
            }

            return criterias.OrderBy(o => o.UseOr).ToList();
        }

        private static Type GetNonNullable(Type propertyType)
        {
            if (IsNullableType(propertyType))
            {
                return Nullable.GetUnderlyingType(propertyType);
            }

            return propertyType;
        }

        private static bool IsNullableType(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}
