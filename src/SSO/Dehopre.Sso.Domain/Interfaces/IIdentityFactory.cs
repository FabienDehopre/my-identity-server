namespace Dehopre.Sso.Domain.Interfaces
{
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Commands.UserManagement;

    public interface IIdentityFactory<TUser> where TUser : class
    {
        void UpdateInfo(AdminUpdateUserCommand command, TUser user);
        void UpdateProfile(UpdateProfileCommand command, TUser user);
        TUser Create(UserCommand user);
    }

    public interface IRoleFactory<TRole> where TRole : class
    {
        TRole CreateRole(string name);
    }
}
