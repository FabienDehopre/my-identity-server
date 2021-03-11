namespace Dehopre.Sso.Domain.Events.User
{
    using Dehopre.Domain.Core.Events;

    public class UserLoginRemovedEvent : Event
    {
        public string Username { get; }
        public string LoginProvider { get; }
        public string ProviderKey { get; }

        public UserLoginRemovedEvent(string username, string loginProvider, string providerKey)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Username = username;
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
        }
    }
}
