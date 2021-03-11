namespace Dehopre.Sso.Domain.Commands.UserManagement
{
    using Dehopre.Sso.Domain.Validations.UserManagement;

    public class RemoveAccountCommand : ProfileCommand
    {
        public RemoveAccountCommand(string username) => this.Username = username;

        public override bool IsValid()
        {
            this.ValidationResult = new RemoveAccountCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
