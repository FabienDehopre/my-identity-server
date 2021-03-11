namespace Dehopre.Sso.Domain.Validations.Role
{
    using Dehopre.Sso.Domain.Commands.Role;

    public class RemoveUserFromRoleValidation : RoleValidation<RemoveUserFromRoleCommand>
    {
        public RemoveUserFromRoleValidation()
        {
            this.ValidateName();
            this.ValidateUsername();
        }
    }
}
