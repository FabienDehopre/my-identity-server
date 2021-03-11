namespace Dehopre.Sso.Domain.Events.Role
{
    using Dehopre.Domain.Core.Events;

    public class RoleRemovedEvent : Event
    {
        public RoleRemovedEvent(string name) : base(EventTypes.Success) => this.AggregateId = name;
    }
}
