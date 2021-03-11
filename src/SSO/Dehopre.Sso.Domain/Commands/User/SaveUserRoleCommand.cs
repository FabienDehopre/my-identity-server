namespace Dehopre.Sso.Domain.Commands.User
{
    using Dehopre.Sso.Domain.Validations.User;

    public class SaveUserRoleCommand : UserRoleCommand
    {
        public SaveUserRoleCommand(string username, string role)
        {
            this.Role = role;
            this.Username = username;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new SaveUserRoleCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
