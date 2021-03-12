namespace Dehopre.Sso.AspNetIdentity.Services
{
    using Dehopre.Sso.AspNetIdentity.Models.Identity;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using Dehopre.Sso.Domain.Interfaces;

    public class IdentityFactory : IIdentityFactory<UserIdentity>, IRoleFactory<RoleIdentity>
    {
        public UserIdentity Create(UserCommand user) => new()
        {
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            UserName = user.Username,
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnd = null,
        };

        public RoleIdentity CreateRole(string name) => new(name);


        public void UpdateInfo(AdminUpdateUserCommand command, UserIdentity userDb)
        {
            userDb.Email = command.Email;
            userDb.EmailConfirmed = command.EmailConfirmed;
            userDb.AccessFailedCount = command.AccessFailedCount;
            userDb.LockoutEnabled = command.LockoutEnabled;
            userDb.LockoutEnd = command.LockoutEnd;
            userDb.TwoFactorEnabled = command.TwoFactorEnabled;
            userDb.PhoneNumber = command.PhoneNumber;
            userDb.PhoneNumberConfirmed = command.PhoneNumberConfirmed;
        }

        public void UpdateProfile(UpdateProfileCommand command, UserIdentity user) => user.PhoneNumber = command.PhoneNumber;

    }
}
