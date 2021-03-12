namespace Dehopre.Sso.AspNetIdentity.Models.Identity
{
    using Microsoft.AspNetCore.Identity;

    public class RoleIdentity : IdentityRole
    {
        public RoleIdentity() : base()
        {
        }

        public RoleIdentity(string roleName) : base(roleName)
        {
        }
    }
}
