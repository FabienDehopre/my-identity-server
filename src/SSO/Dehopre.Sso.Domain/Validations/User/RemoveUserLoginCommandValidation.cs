namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class RemoveUserLoginCommandValidation : UserLoginValidation<RemoveUserLoginCommand>
    {
        public RemoveUserLoginCommandValidation()
        {
            this.ValidateUsername();
            this.ValidateLoginProvider();
            this.ValidateProviderKey();
        }
    }
}
