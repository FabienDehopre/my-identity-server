namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;

    public class SetPasswordCommandValidation : PasswordCommandValidation<SetPasswordCommand>
    {
        public SetPasswordCommandValidation()
        {
            this.ValidateUsername();
            this.ValidatePassword();
        }
    }
}
