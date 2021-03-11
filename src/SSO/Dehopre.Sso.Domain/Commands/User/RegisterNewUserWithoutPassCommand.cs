namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class RegisterNewUserWithoutPassCommand : UserCommand
    {
        public RegisterNewUserWithoutPassCommand(string username, string email, string name, string picture, string provider, string providerId, bool checkProvider)
        {
            this.CheckProvider = checkProvider;
            this.Provider = provider;
            this.ProviderId = providerId;
            this.Username = username;
            this.Picture = picture;
            this.Email = email;
            this.Name = name;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new RegisterNewUserWithoutPassCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
