namespace Dehopre.Sso.Domain.Commands.UserManagement
{
    using Dehopre.Sso.Domain.Validations.UserManagement;

    public class ChangePasswordCommand : PasswordCommand
    {

        public ChangePasswordCommand(string username, string oldPassword, string newPassword, string confirmPassword)
        {
            this.Username = username;
            this.OldPassword = oldPassword;
            this.Password = newPassword;
            this.ConfirmPassword = confirmPassword;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new ChangePasswordCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
