namespace Dehopre.Sso.Domain.Models
{
    using System.Linq;
    using Dehopre.Domain.Core.Util;

    public class BlindCarbonCopy
    {
        private string recipientsCollection;

        public BlindCarbonCopy() { }

        public BlindCarbonCopy(string emails) => this.recipientsCollection = emails;

        public string[] Recipients
        {
            get => this.recipientsCollection?.Split(";");
            set => this.recipientsCollection = string.Join(";", value);
        }

        public bool IsValid() => this.Recipients is not null && this.Recipients.Any() && this.Recipients.All(a => a.IsEmail());

        public static implicit operator BlindCarbonCopy(string value) => new(value);

        public override string ToString() => this.recipientsCollection;
    }
}
