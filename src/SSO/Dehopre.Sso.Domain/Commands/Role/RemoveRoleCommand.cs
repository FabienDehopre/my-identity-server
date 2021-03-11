namespace Dehopre.Sso.Domain.Commands.Role
{
    using Dehopre.Sso.Domain.Validations.Role;

    public class RemoveRoleCommand : RoleCommand
    {
        public RemoveRoleCommand(string name) => this.Name = name;

        public override bool IsValid()
        {
            this.ValidationResult = new RemoveRoleCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
