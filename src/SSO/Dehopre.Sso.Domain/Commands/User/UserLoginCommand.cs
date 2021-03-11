namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Domain.Core.Commands;

    public abstract class UserLoginCommand : Command
    {
        public string Username { get; protected set; }
        public string LoginProvider { get; protected set; }
        public string ProviderKey { get; protected set; }
    }
}
