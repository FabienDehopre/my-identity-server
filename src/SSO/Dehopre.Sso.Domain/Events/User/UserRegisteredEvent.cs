namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class UserRegisteredEvent : Event
    {
        public string Username { get; }
        public string UserEmail { get; }

        public UserRegisteredEvent(string username, string userEmail)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Username = username;
            this.UserEmail = userEmail;
        }
    }
}
