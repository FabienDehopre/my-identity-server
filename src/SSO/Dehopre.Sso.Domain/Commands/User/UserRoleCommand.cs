namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Domain.Core.Commands;

    public abstract class UserRoleCommand : Command
    {
        public string Username { get; protected set; }
        public string Role { get; protected set; }
    }
}
