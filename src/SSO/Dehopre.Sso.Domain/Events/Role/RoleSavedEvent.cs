namespace Dehopre.Sso.Domain.Events.Role
{
    using Dehopre.Domain.Core.Events;

    public class RoleSavedEvent : Event
    {
        public RoleSavedEvent(string name) : base(EventTypes.Success) => this.AggregateId = name;
    }
}
