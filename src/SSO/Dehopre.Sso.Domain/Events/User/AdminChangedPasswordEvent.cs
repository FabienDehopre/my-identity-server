namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class AdminChangedPasswordEvent : Event
    {
        public AdminChangedPasswordEvent(string username) : base(EventTypes.Success) => this.AggregateId = username;
    }
}
