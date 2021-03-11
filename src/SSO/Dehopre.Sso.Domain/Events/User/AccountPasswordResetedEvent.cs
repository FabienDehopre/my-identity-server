namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class AccountPasswordResetedEvent : Event
    {
        public string Email { get; }
        public string Code { get; }

        public AccountPasswordResetedEvent(string username, string email, string code)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Email = email;
            this.Code = code;
        }
    }
}
