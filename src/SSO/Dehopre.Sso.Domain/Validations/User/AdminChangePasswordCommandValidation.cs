namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class AdminChangePasswordCommandValidation : UserValidation<AdminChangePasswordCommand>
    {
        public AdminChangePasswordCommandValidation() => this.ValidateUsername();
    }
}
