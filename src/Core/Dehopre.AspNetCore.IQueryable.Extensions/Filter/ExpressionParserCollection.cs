namespace Dehopre.AspNetCore.IQueryable.Extensions.Filter
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ExpressionParserCollection : List<ExpressionParser>
    {
        public ParameterExpression ParameterExpression { get; set; }

        public List<ExpressionParser> Ordered() => this.OrderBy(b => b.Criteria.UseOr).ToList();
    }
}
