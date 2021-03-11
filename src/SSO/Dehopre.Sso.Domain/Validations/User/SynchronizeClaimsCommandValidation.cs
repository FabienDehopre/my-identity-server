namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class SynchronizeClaimsCommandValidation : UserClaimValidation<SynchronizeClaimsCommand>
    {
        public SynchronizeClaimsCommandValidation()
        {
            this.ValidateUsername();
            this.ValidateClaims();
        }
    }
}
