namespace Dehopre.Sso.Domain.Models
{
    public class UserLogin
    {
        public UserLogin(string loginProvider, string providerDisplayName, string providerKey)
        {
            this.LoginProvider = loginProvider;
            this.ProviderDisplayName = providerDisplayName;
            this.ProviderKey = providerKey;
        }

        public string LoginProvider { get; private set; }
        public string ProviderDisplayName { get; private set; }
        public string ProviderKey { get; private set; }
    }
}
