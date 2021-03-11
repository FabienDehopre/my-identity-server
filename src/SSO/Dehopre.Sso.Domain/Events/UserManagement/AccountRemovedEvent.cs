namespace Dehopre.Sso.Domain.Events.UserManagement
{
    using Dehopre.Domain.Core.Events;

    public class AccountRemovedEvent : Event
    {
        public AccountRemovedEvent(string username) : base(EventTypes.Success) => this.AggregateId = username;
    }
}
