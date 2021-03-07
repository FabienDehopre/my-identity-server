namespace Dehopre.AspNetCore.IQueryable.Extensions.Attributes
{
    using System;
    using Dehopre.AspNetCore.IQueryable.Extensions.Filter;

    [AttributeUsage(AttributeTargets.Property)]
    public class QueryOperatorAttribute : Attribute
    {
        public WhereOperator Operator { get; set; } = WhereOperator.Equals;
        public bool UseNot { get; set; } = false;
        public bool CaseSensitive { get; set; } = true;
        public string HasName { get; set; }
        public int Max { get; set; }
        public bool UseOr { get; set; } = false;
    }
}
