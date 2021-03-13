namespace Dehopre.Sso.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.ViewModels;
    using Dehopre.Sso.Application.EventSourcedNormalizers;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Application.ViewModels.RoleViewModels;
    using Dehopre.Sso.Application.ViewModels.UserViewModels;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.ViewModels.User;

    public interface IUserManageAppService : IDisposable
    {
        Task<UserViewModel> FindByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<UserViewModel> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<UserViewModel> FindByProviderAsync(string provider, string providerUserId, CancellationToken cancellationToken = default);
        Task<ListOf<EventHistoryData>> GetEvents(string username, PagingViewModel paging, CancellationToken cancellationToken = default);
        Task<UserViewModel> GetUserDetails(string username, CancellationToken cancellationToken = default);
        Task<IEnumerable<ClaimViewModel>> GetClaims(string userName, CancellationToken cancellationToken = default);
        Task<bool> SynchronizeClaims(string username, IEnumerable<ClaimViewModel> claims, CancellationToken cancellationToken = default);
        Task<IEnumerable<RoleViewModel>> GetRoles(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserLoginViewModel>> GetLogins(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserListViewModel>> GetUsersInRole(string role, CancellationToken cancellationToken = default);
        Task<ListOf<UserListViewModel>> SearchUsersByProperties(UserFindByProperties findByProperties, CancellationToken cancellationToken = default);
        Task<ListOf<UserListViewModel>> Search(IUserSearch search, CancellationToken cancellationToken = default);
        Task<bool> UpdateProfile(UserViewModel model, CancellationToken cancellationToken = default);
        Task<bool> UpdateProfilePicture(ProfilePictureViewModel model, CancellationToken cancellationToken = default);
        Task<bool> ChangePassword(ChangePasswordViewModel model, CancellationToken cancellationToken = default);
        Task<bool> CreatePassword(SetPasswordViewModel model, CancellationToken cancellationToken = default);
        Task<bool> RemoveAccount(RemoveAccountViewModel model, CancellationToken cancellationToken = default);
        Task<bool> HasPassword(string userId, CancellationToken cancellationToken = default);
        Task<bool> UpdateUser(UserViewModel model, CancellationToken cancellationToken = default);
        Task<bool> SaveClaim(SaveUserClaimViewModel model, CancellationToken cancellationToken = default);
        Task<bool> RemoveClaim(RemoveUserClaimViewModel model, CancellationToken cancellationToken = default);
        Task<bool> RemoveRole(RemoveUserRoleViewModel model, CancellationToken cancellationToken = default);
        Task<bool> SaveRole(SaveUserRoleViewModel model, CancellationToken cancellationToken = default);
        Task<bool> RemoveLogin(RemoveUserLoginViewModel model, CancellationToken cancellationToken = default);
        Task<bool> ResetPassword(AdminChangePasswordViewodel model, CancellationToken cancellationToken = default);
    }
}
