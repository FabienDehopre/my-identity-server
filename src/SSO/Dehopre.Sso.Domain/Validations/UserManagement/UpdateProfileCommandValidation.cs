namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;

    public class UpdateProfileCommandValidation : ProfileValidation<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidation() => this.ValidateUsername();
    }
}
