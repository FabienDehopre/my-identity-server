namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class RegisterNewUserCommandValidation : UserValidation<RegisterNewUserCommand>
    {
        public RegisterNewUserCommandValidation()
        {
            this.ValidateName();
            this.ValidateUsername();
            this.ValidateEmail();
            this.ValidatePassword();

        }
    }
}
