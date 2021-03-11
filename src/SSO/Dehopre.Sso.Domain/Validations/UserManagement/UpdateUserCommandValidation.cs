namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;

    public class UpdateUserCommandValidation : UserManagementValidation<UserManagementCommand>
    {
        public UpdateUserCommandValidation()
        {
            this.ValidateEmail();
            this.ValidateName();
            this.ValidateUsername();
        }

    }
}
