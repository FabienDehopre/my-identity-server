namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;

    public class RemoveAccountCommandValidation : ProfileValidation<RemoveAccountCommand>
    {
        public RemoveAccountCommandValidation() => this.ValidateUsername();
    }
}
