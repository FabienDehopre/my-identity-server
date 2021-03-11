namespace Dehopre.Sso.Domain.Validations.Role
{
    using Dehopre.Sso.Domain.Commands.Role;

    public class UpdateRoleCommandValidation : RoleValidation<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidation()
        {
            this.ValidateName();
            this.ValidateNewName();
        }
    }
}
