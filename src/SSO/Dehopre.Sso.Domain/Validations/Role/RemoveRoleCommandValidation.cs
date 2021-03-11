namespace Dehopre.Sso.Domain.Validations.Role
{
    using Dehopre.Sso.Domain.Commands.Role;

    public class RemoveRoleCommandValidation : RoleValidation<RemoveRoleCommand>
    {
        public RemoveRoleCommandValidation() => this.ValidateName();
    }
}
