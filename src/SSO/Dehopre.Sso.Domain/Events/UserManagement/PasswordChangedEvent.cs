namespace Dehopre.Sso.Domain.Events.UserManagement
{
    using Dehopre.Domain.Core.Events;

    public class PasswordChangedEvent : Event
    {
        public PasswordChangedEvent(string username) : base(EventTypes.Success) => this.AggregateId = username;
    }
}
