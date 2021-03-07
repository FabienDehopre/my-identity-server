namespace Dehopre.AspNetCore.IQueryable.Extensions.Filter
{
    using System.Linq.Expressions;

    public class ExpressionParser
    {
        public WhereClause Criteria { get; set; }
        public Expression FieldToFilter { get; set; }
        public Expression FiterBy { get; set; }
    }
}
