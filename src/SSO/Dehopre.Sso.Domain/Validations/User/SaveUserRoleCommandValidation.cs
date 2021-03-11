namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class SaveUserRoleCommandValidation : UserRoleValidation<SaveUserRoleCommand>
    {
        public SaveUserRoleCommandValidation()
        {
            this.ValidateUsername();
            this.ValidateRole();
        }
    }
}
