namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;

    public class UpdateProfilePictureCommandValidation : ProfileValidation<UpdateProfilePictureCommand>
    {
        public UpdateProfilePictureCommandValidation() => this.ValidatePicture();
    }
}
