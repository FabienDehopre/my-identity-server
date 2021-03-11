namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class UserRoleRemovedEvent : Event
    {
        public string Username { get; }
        public string Role { get; }

        public UserRoleRemovedEvent(string username, string role)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Username = username;
            this.Role = role;
        }
    }
}
