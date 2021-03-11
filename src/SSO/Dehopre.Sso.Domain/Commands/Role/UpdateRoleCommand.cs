namespace Dehopre.Sso.Domain.Commands.Role
{
    using Dehopre.Sso.Domain.Validations.Role;

    public class UpdateRoleCommand : RoleCommand
    {
        public UpdateRoleCommand(string name, string oldName)
        {
            this.OldName = oldName;
            this.Name = name;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new UpdateRoleCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
