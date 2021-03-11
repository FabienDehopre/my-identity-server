namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class RemoveUserRoleCommandValidation : UserRoleValidation<RemoveUserRoleCommand>
    {
        public RemoveUserRoleCommandValidation()
        {
            this.ValidateUsername();
            this.ValidateRole();
        }
    }
}
