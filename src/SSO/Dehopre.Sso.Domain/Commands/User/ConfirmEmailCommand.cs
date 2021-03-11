namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class ConfirmEmailCommand : UserCommand
    {

        public ConfirmEmailCommand(string code, string email)
        {
            this.Code = code;
            this.Email = email;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new ConfirmEmailCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
