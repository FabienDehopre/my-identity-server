namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class SaveUserClaimCommand : UserClaimCommand
    {
        public SaveUserClaimCommand(string username, string type, string value)
        {
            this.Type = type;
            this.Username = username;
            this.Value = value;
        }
        public override bool IsValid()
        {
            this.ValidationResult = new SaveUserClaimCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
