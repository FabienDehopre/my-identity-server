namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class SendResetLinkCommand : UserCommand
    {
        public SendResetLinkCommand(string emailOrUsername) => this.EmailOrUsername = emailOrUsername;

        public override bool IsValid()
        {
            this.ValidationResult = new SendResetLinkCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
