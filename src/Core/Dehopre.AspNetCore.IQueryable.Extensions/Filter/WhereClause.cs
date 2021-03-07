namespace Dehopre.AspNetCore.IQueryable.Extensions.Filter
{
    using System.Diagnostics;
    using System.Reflection;
    using Dehopre.AspNetCore.IQueryable.Extensions.Attributes;

    [DebuggerDisplay("{FieldName}")]
    public class WhereClause
    {
        private bool customName;

        public WhereOperator Operator { get; set; }
        public bool CaseSensitive { get; set; }
        public bool UseNot { get; set; }
        public PropertyInfo Property { get; set; }
        public string FieldName { get; set; }
        public bool UseOr { get; set; }

        public WhereClause()
        {
            this.Operator = WhereOperator.Equals;
            this.UseNot = false;
            this.CaseSensitive = true;
            this.UseOr = false;
        }

        public void UpdateAttributeData(QueryOperatorAttribute data)
        {
            this.Operator = data.Operator;
            this.CaseSensitive = data.CaseSensitive;
            this.UseNot = data.UseNot;
            this.FieldName = data.HasName;
            this.UseOr = data.UseOr;
            if (!string.IsNullOrWhiteSpace(this.FieldName))
            {
                this.customName = true;
            }
        }

        public void UpdateValues(PropertyInfo propertyInfo)
        {
            this.Property = propertyInfo;
            if (!this.customName)
            {
                this.FieldName = this.Property.Name;
            }
        }
    }
}
