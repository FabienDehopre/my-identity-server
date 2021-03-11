namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class AdminChangePasswordCommand : UserCommand
    {
        public AdminChangePasswordCommand(string password, string changePassword, string username)
        {
            this.Username = username;
            this.Password = password;
            this.ConfirmPassword = changePassword;
        }
        public override bool IsValid()
        {
            this.ValidationResult = new AdminChangePasswordCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
