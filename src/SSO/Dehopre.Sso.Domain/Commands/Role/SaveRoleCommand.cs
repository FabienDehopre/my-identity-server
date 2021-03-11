namespace Dehopre.Sso.Domain.Commands.Role
{
    using Dehopre.Sso.Domain.Validations.Role;

    public class SaveRoleCommand : RoleCommand
    {
        public SaveRoleCommand(string name) => this.Name = name;

        public override bool IsValid()
        {
            this.ValidationResult = new SaveRoleCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
