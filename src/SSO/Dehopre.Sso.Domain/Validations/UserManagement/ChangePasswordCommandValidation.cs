namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;

    public class ChangePasswordCommandValidation : PasswordCommandValidation<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidation()
        {
            this.ValidateUsername();
            this.ValidateOldPassword();
            this.ValidatePassword();
        }
    }
}
