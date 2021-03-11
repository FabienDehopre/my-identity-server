namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class AddLoginCommand : UserCommand
    {
        public AddLoginCommand(string email, string provider, string providerId)
        {
            this.Provider = provider;
            this.ProviderId = providerId;
            this.Email = email;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new AddLoginCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
