namespace Dehopre.Sso.Domain.Commands.UserManagement
{
    using Dehopre.Domain.Core.Commands;

    public abstract class PasswordCommand : Command
    {
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string ConfirmPassword { get; protected set; }
        public string OldPassword { get; protected set; }
    }
}
