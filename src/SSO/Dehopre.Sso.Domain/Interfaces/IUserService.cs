namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Dehopre.AspNetCore.IQueryable.Extensions;
    using Dehopre.AspNetCore.IQueryable.Extensions.Pagination;
    using Dehopre.AspNetCore.IQueryable.Extensions.Sort;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using Dehopre.Sso.Domain.Models;
    using Dehopre.Sso.Domain.ViewModels.User;

    public interface IUserService
    {
        Task<AccountResult?> CreateUserWithPass(RegisterNewUserCommand user, string password);
        Task<AccountResult?> CreateUserWithouthPassword(RegisterNewUserWithoutPassCommand user);
        Task<AccountResult?> CreateUserWithProviderAndPass(RegisterNewUserWithProviderCommand user);
        Task<AccountResult?> GenerateResetPasswordLink(string emailOrUsername);
        Task<IDomainUser> FindByEmailAsync(string email);
        Task<IDomainUser> FindByNameAsync(string username);
        Task<IDomainUser> FindByProviderAsync(string provider, string providerUserId);
        Task<IEnumerable<Claim>> GetClaimByName(string userName);
        Task<IEnumerable<UserLogin>> GetUserLogins(string userName);
        Task<IEnumerable<string>> GetRoles(string userName);
        Task<IEnumerable<IDomainUser>> GetUserFromRole(string role);
        Task<IDomainUser> FindByUsernameOrEmail(string emailOrUsername);
        Task<bool> UsernameExist(string userName);
        Task<bool> EmailExist(string email);
        Task<string> ConfirmEmailAsync(string email, string code);
        Task<bool> UpdateProfileAsync(UpdateProfileCommand command);
        Task<bool> UpdateProfilePictureAsync(UpdateProfilePictureCommand command);
        Task<bool> UpdateUserAsync(AdminUpdateUserCommand user);
        Task<bool> CreatePasswordAsync(SetPasswordCommand request);
        Task<bool> ChangePasswordAsync(ChangePasswordCommand request);
        Task<bool> RemoveAccountAsync(RemoveAccountCommand request);
        Task<bool> HasPassword(string username);
        Task<bool> SaveClaim(string username, Claim claim);
        Task<bool> RemoveClaim(string username, string claimType, string value);
        Task<bool> RemoveRole(string username, string requestRole);
        Task<bool> SaveRole(string username, string role);
        Task<bool> RemoveLogin(string username, string loginProvider, string providerKey);
        Task<bool> RemoveUserFromRole(string name, string username);
        Task<bool> ResetPasswordAsync(string username, string password);
        Task<string> ResetPassword(string email, string code, string password);
        Task<string> AddLoginAsync(string email, string provider, string providerId);
        Task<int> CountByProperties(string query);
        Task<IEnumerable<IDomainUser>> SearchByProperties(string query, IQuerySort sort, IQueryPaging paging);
        Task<IEnumerable<IDomainUser>> Search(ICustomQueryable search);
        Task<int> Count(ICustomQueryable search);
        Task<Dictionary<Username, IEnumerable<Claim>>> GetClaimsFromUsers(IEnumerable<string> username, params string[] claimType);
    }
}
