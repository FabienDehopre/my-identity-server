namespace Dehopre.Sso.Domain.Commands.UserManagement
{
    using Dehopre.Sso.Domain.Validations.UserManagement;

    public class UpdateProfilePictureCommand : ProfileCommand
    {
        public UpdateProfilePictureCommand(string username, string picture)
        {
            this.Username = username;
            this.Picture = picture;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new UpdateProfilePictureCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
