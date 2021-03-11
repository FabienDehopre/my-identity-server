namespace Dehopre.Sso.Domain.Events.Role
{
    using Dehopre.Domain.Core.Events;

    public class RoleUpdatedEvent : Event
    {
        public string Name { get; }
        public string OldName { get; }

        public RoleUpdatedEvent(string name, string oldName)
            : base(EventTypes.Success)
        {
            this.AggregateId = name;
            this.Name = name;
            this.OldName = oldName;
        }
    }
}
