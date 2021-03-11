namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class NewLoginAddedEvent : Event
    {
        public string Email { get; }
        public string Provider { get; }
        public string ProviderId { get; }

        public NewLoginAddedEvent(string username, string email, string provider, string providerId)
            : base(EventTypes.Success)
        {
            this.Email = email;
            this.Provider = provider;
            this.ProviderId = providerId;
            this.AggregateId = username;
        }
    }
}
