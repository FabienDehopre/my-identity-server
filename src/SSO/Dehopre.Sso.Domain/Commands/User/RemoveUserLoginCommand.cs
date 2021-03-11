namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class RemoveUserLoginCommand : UserLoginCommand
    {
        public RemoveUserLoginCommand(string username, string loginProvider, string providerKey)
        {
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
            this.Username = username;
        }
        public override bool IsValid()
        {
            this.ValidationResult = new RemoveUserLoginCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
