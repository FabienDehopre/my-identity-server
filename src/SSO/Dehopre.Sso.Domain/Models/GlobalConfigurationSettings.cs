namespace Dehopre.Sso.Domain.Models
{
    using System;
    using Dehopre.Domain.Core.Models;

    public class GlobalConfigurationSettings : Entity
    {
        public GlobalConfigurationSettings() => this.Id = Guid.NewGuid();

        public GlobalConfigurationSettings(string key, string value, bool sensitive, bool isPublic)
            : this()
        {
            this.Key = key;
            this.Value = value;
            this.Sensitive = sensitive;
            this.IsPublic = isPublic;
        }

        public string Key { get; private set; }
        public string Value { get; private set; }
        public bool Sensitive { get; private set; }
        public bool IsPublic { get; }
        public bool Public { get; private set; }

        public void Update(string value, in bool isPublic, in bool sensitive)
        {
            if (value.Contains("Sensitive Data"))
            {
                return;
            }

            this.Value = value;
            this.Public = isPublic;
            this.Sensitive = sensitive;
        }
    }
}
