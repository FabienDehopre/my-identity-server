namespace Dehopre.Sso.AspNetIdentity.Models.Identity
{
    using Dehopre.Domain.Core.Interfaces;
    using Microsoft.AspNetCore.Identity;

    public class UserIdentity : IdentityUser, IDomainUser
    {
        public void ConfirmEmail() => this.EmailConfirmed = true;
    }
}
