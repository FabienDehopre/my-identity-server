namespace Dehopre.Sso.Domain.Events.GlobalConfiguration
{
    using Dehopre.Domain.Core.Events;

    public class GlobalConfigurationUpdatedEvent : Event
    {
        public string Key { get; }
        public string Value { get; }
        public bool IsPublic { get; }
        public bool Sensitive { get; }

        public GlobalConfigurationUpdatedEvent(string key, string value, in bool isPublic, in bool sensitive)
        {
            this.AggregateId = key;
            this.Key = key;
            this.Value = value;
            this.IsPublic = isPublic;
            this.Sensitive = sensitive;
        }
    }
}
