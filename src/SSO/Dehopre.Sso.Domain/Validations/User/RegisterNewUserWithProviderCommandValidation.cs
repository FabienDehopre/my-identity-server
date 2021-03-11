namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class RegisterNewUserWithProviderCommandValidation : UserValidation<RegisterNewUserWithProviderCommand>
    {
        public RegisterNewUserWithProviderCommandValidation()
        {
            this.ValidateName();
            this.ValidateUsername();
            this.ValidateEmail();
            this.ValidateProvider();
            this.ValidateProviderId();
        }
    }
}
