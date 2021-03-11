namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class EmailConfirmedEvent : Event
    {
        public string Email { get; }
        public string Code { get; }

        public EmailConfirmedEvent(string email, string code, string username)
            : base(EventTypes.Success)
        {
            this.Email = email;
            this.Code = code;
            this.AggregateId = username;
        }
    }
}
