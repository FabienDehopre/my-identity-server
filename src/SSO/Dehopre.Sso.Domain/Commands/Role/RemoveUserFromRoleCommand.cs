namespace Dehopre.Sso.Domain.Commands.Role
{
    using Dehopre.Sso.Domain.Validations.Role;

    public class RemoveUserFromRoleCommand : RoleCommand
    {
        public RemoveUserFromRoleCommand(string roleName, string username)
        {
            this.Username = username;
            this.Name = roleName;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new RemoveUserFromRoleValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
