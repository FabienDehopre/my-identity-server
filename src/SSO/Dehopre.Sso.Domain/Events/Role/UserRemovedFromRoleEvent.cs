namespace Dehopre.Sso.Domain.Events.Role
{
    using Dehopre.Domain.Core.Events;

    public class UserRemovedFromRoleEvent : Event
    {
        public string Name { get; }
        public string Username { get; }

        public UserRemovedFromRoleEvent(string name, string username)
            : base(EventTypes.Success)
        {
            this.AggregateId = name;
            this.Name = name;
            this.Username = username;
        }
    }
}
