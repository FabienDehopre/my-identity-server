namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class RemoveUserClaimCommandValidation : UserClaimValidation<RemoveUserClaimCommand>
    {
        public RemoveUserClaimCommandValidation()
        {
            this.ValidateUsername();
            this.ValidateKey();
        }
    }
}
