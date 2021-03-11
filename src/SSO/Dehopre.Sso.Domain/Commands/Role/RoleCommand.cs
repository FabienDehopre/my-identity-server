namespace Dehopre.Sso.Domain.Commands.Role
{
    using Dehopre.Domain.Core.Commands;

    public abstract class RoleCommand : Command
    {
        public string OldName { get; protected set; }
        public string Name { get; protected set; }
        public string Username { get; protected set; }
    }
}
