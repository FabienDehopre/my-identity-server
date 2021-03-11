namespace Dehopre.Sso.Domain.Commands.UserManagement
{
    using Dehopre.Sso.Domain.Validations.UserManagement;

    public class SetPasswordCommand : PasswordCommand
    {
        public SetPasswordCommand(string username, string newPassword, string confirmPassword)
        {
            this.Username = username;
            this.Password = newPassword;
            this.ConfirmPassword = confirmPassword;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new SetPasswordCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
