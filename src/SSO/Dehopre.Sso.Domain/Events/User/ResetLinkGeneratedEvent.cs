namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class ResetLinkGeneratedEvent : Event
    {
        public string Email { get; }
        public string Username { get; }

        public ResetLinkGeneratedEvent(string email, string username)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Email = email;
            this.Username = username;
        }
    }
}
