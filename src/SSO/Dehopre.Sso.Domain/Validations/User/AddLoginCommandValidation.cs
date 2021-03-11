namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class AddLoginCommandValidation : UserValidation<AddLoginCommand>
    {
        public AddLoginCommandValidation()
        {
            this.ValidateEmail();
            this.ValidateProvider();
            this.ValidateProviderId();
        }

    }
}
