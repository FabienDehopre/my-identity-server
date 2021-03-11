namespace Dehopre.Sso.Domain.Validations.Role
{
    using Dehopre.Sso.Domain.Commands.Role;

    public class SaveRoleCommandValidation : RoleValidation<SaveRoleCommand>
    {
        public SaveRoleCommandValidation() => this.ValidateName();
    }
}
