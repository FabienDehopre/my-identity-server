namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class RegisterNewUserWithoutPassCommandValidation : UserValidation<RegisterNewUserWithoutPassCommand>
    {
        public RegisterNewUserWithoutPassCommandValidation()
        {
            this.ValidateName();
            this.ValidateUsername();
            this.ValidateEmail();
            this.ValidateProvider();
            this.ValidateProviderId();
        }
    }
}
