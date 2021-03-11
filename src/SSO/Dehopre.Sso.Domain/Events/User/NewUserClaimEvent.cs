namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class NewUserClaimEvent : Event
    {
        public string Username { get; }
        public string Type { get; }
        public string Value { get; }

        public NewUserClaimEvent(string username, string type, string value)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Username = username;
            this.Type = type;
            this.Value = value;
        }
    }
}
