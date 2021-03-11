namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class ResetPasswordCommand : UserCommand
    {
        public ResetPasswordCommand(string password, string confirmPassword, string code, string email)
        {
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
            this.Code = code;
            this.Email = email;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new ResetPasswordCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
