namespace Dehopre.Sso.Domain.Events.UserManagement
{
    using Dehopre.Domain.Core.Events;

    public class PasswordCreatedEvent : Event
    {
        public PasswordCreatedEvent(string username) : base(EventTypes.Success) => this.AggregateId = username;
    }
}
