namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading;
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
        Task<AccountResult?> CreateUserWithPass(RegisterNewUserCommand user, string password, CancellationToken cancellationToken = default);
        Task<AccountResult?> CreateUserWithouthPassword(RegisterNewUserWithoutPassCommand user, CancellationToken cancellationToken = default);
        Task<AccountResult?> CreateUserWithProviderAndPass(RegisterNewUserWithProviderCommand user, CancellationToken cancellationToken = default);
        Task<AccountResult?> GenerateResetPasswordLink(string emailOrUsername, CancellationToken cancellationToken = default);
        Task<IDomainUser> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IDomainUser> FindByNameAsync(string username, CancellationToken cancellationToken = default);
        Task<IDomainUser> FindByProviderAsync(string provider, string providerUserId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Claim>> GetClaimByName(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserLogin>> GetUserLogins(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetRoles(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<IDomainUser>> GetUserFromRole(string role, CancellationToken cancellationToken = default);
        Task<IDomainUser> FindByUsernameOrEmail(string emailOrUsername, CancellationToken cancellationToken = default);
        Task<bool> UsernameExist(string userName, CancellationToken cancellationToken = default);
        Task<bool> EmailExist(string email, CancellationToken cancellationToken = default);
        Task<string> ConfirmEmailAsync(string email, string code, CancellationToken cancellationToken = default);
        Task<bool> UpdateProfileAsync(UpdateProfileCommand command, CancellationToken cancellationToken = default);
        Task<bool> UpdateProfilePictureAsync(UpdateProfilePictureCommand command, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserAsync(AdminUpdateUserCommand user, CancellationToken cancellationToken = default);
        Task<bool> CreatePasswordAsync(SetPasswordCommand request, CancellationToken cancellationToken = default);
        Task<bool> ChangePasswordAsync(ChangePasswordCommand request, CancellationToken cancellationToken = default);
        Task<bool> RemoveAccountAsync(RemoveAccountCommand request, CancellationToken cancellationToken = default);
        Task<bool> HasPassword(string username, CancellationToken cancellationToken = default);
        Task<bool> SaveClaim(string username, Claim claim, CancellationToken cancellationToken = default);
        Task<bool> RemoveClaim(string username, string claimType, string value, CancellationToken cancellationToken = default);
        Task<bool> RemoveRole(string username, string requestRole, CancellationToken cancellationToken = default);
        Task<bool> SaveRole(string username, string role, CancellationToken cancellationToken = default);
        Task<bool> RemoveLogin(string username, string loginProvider, string providerKey, CancellationToken cancellationToken = default);
        Task<bool> RemoveUserFromRole(string name, string username, CancellationToken cancellationToken = default);
        Task<bool> ResetPasswordAsync(string username, string password, CancellationToken cancellationToken = default);
        Task<string> ResetPassword(string email, string code, string password, CancellationToken cancellationToken = default);
        Task<string> AddLoginAsync(string email, string provider, string providerId, CancellationToken cancellationToken = default);
        Task<int> CountByProperties(string query, CancellationToken cancellationToken = default);
        Task<IEnumerable<IDomainUser>> SearchByProperties(string query, IQuerySort sort, IQueryPaging paging, CancellationToken cancellationToken = default);
        Task<IEnumerable<IDomainUser>> Search(ICustomQueryable search, CancellationToken cancellationToken = default);
        Task<int> Count(ICustomQueryable search, CancellationToken cancellationToken = default);
        Task<Dictionary<Username, IEnumerable<Claim>>> GetClaimsFromUsers(IEnumerable<string> username, CancellationToken cancellationToken = default, params string[] claimType);
        Task<Dictionary<Username, IEnumerable<Claim>>> GetClaimsFromUsers(IEnumerable<string> username, params string[] claimType);
    }
}
