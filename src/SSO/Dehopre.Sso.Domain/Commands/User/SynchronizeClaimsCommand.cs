namespace Dehopre.Sso.Domain.Commands.User
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Dehopre.Sso.Domain.Validations.User;

    public class SynchronizeClaimsCommand : UserClaimCommand
    {
        public SynchronizeClaimsCommand(string username, IEnumerable<Claim> claims)
        {
            this.Claims = claims;
            this.Username = username;
        }
        public override bool IsValid()
        {
            this.ValidationResult = new SynchronizeClaimsCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
