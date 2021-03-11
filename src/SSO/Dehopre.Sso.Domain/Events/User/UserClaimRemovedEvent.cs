namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class UserClaimRemovedEvent : Event
    {
        public string Username { get; }
        public string Type { get; }

        public UserClaimRemovedEvent(string username, string type)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Username = username;
            this.Type = type;
        }
    }
}
