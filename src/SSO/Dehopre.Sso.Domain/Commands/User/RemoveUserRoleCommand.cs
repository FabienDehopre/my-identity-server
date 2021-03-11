namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class RemoveUserRoleCommand : UserRoleCommand
    {
        public RemoveUserRoleCommand(string username, string role)
        {
            this.Role = role;
            this.Username = username;
        }
        public override bool IsValid()
        {
            this.ValidationResult = new RemoveUserRoleCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
