namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class RemoveUserClaimCommand : UserClaimCommand
    {

        public RemoveUserClaimCommand(string username, string type, string value)
        {
            this.Value = value;
            this.Type = type;
            this.Username = username;
        }
        public override bool IsValid()
        {
            this.ValidationResult = new RemoveUserClaimCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
