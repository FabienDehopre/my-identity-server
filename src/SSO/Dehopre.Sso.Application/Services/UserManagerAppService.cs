namespace Dehopre.Sso.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.ViewModels;
    using Dehopre.Sso.Application.AutoMapper;
    using Dehopre.Sso.Application.EventSourcedNormalizers;
    using Dehopre.Sso.Application.Interfaces;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Application.ViewModels.RoleViewModels;
    using Dehopre.Sso.Application.ViewModels.UserViewModels;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.ViewModels;
    using Dehopre.Sso.Domain.ViewModels.User;
    using global::AutoMapper;

    public class UserManagerAppService : IUserManageAppService
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IEventStoreRepository eventStoreRepository;
        private readonly IStorage storage;
        private readonly IMediatorHandler bus;

        public UserManagerAppService(
            IUserService userService,
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository,
            IStorage storage
            )
        {
            this.mapper = UserMapping.Mapper;
            this.userService = userService;
            this.bus = bus;
            this.eventStoreRepository = eventStoreRepository;
            this.storage = storage;
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<UserViewModel> FindByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var user = await this.userService.FindByNameAsync(username, cancellationToken);
            var userVo = this.mapper.Map<UserViewModel>(user);
            return await this.GetUserMetadata(userVo, cancellationToken);
        }

        private async Task<UserViewModel> GetUserMetadata(UserViewModel user, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(user?.UserName))
            {
                return null;
            }

            var claims = await this.userService.GetClaimByName(user?.UserName, cancellationToken);
            user?.UpdateMetadata(claims.ToList());
            return user;
        }

        public async Task<UserViewModel> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await this.userService.FindByEmailAsync(email, cancellationToken);
            var userVo = this.mapper.Map<UserViewModel>(user);
            return await this.GetUserMetadata(userVo, cancellationToken);
        }

        public async Task<UserViewModel> FindByProviderAsync(string provider, string providerUserId, CancellationToken cancellationToken = default)
        {
            var user = await this.userService.FindByProviderAsync(provider, providerUserId, cancellationToken);
            var userVo = this.mapper.Map<UserViewModel>(user);
            return await this.GetUserMetadata(userVo, cancellationToken);
        }

        public async Task<UserViewModel> GetUserDetails(string username, CancellationToken cancellationToken = default)
        {
            var users = await this.userService.FindByNameAsync(username, cancellationToken);
            var userVo = this.mapper.Map<UserViewModel>(users);
            return await this.GetUserMetadata(userVo, cancellationToken);
        }

        public async Task<IEnumerable<ClaimViewModel>> GetClaims(string userName, CancellationToken cancellationToken = default) => this.mapper.Map<IEnumerable<ClaimViewModel>>(await this.userService.GetClaimByName(userName, cancellationToken));

        public Task<bool> SynchronizeClaims(string username, IEnumerable<ClaimViewModel> claims, CancellationToken cancellationToken = default) => this.bus.SendCommand(new SynchronizeClaimsCommand(username, claims.Select(s => new Claim(s.Type, s.Value))), cancellationToken);

        public async Task<IEnumerable<RoleViewModel>> GetRoles(string userName, CancellationToken cancellationToken = default)
        {
            var roles = await this.userService.GetRoles(userName, cancellationToken);
            return roles.Select(s => new RoleViewModel() { Name = s });
        }

        public async Task<IEnumerable<UserLoginViewModel>> GetLogins(string userName, CancellationToken cancellationToken = default) => this.mapper.Map<IEnumerable<UserLoginViewModel>>(await this.userService.GetUserLogins(userName, cancellationToken));

        public async Task<IEnumerable<UserListViewModel>> GetUsersInRole(string role, CancellationToken cancellationToken = default) => this.mapper.Map<IEnumerable<UserListViewModel>>(await this.userService.GetUserFromRole(role, cancellationToken));

        public async Task<ListOf<EventHistoryData>> GetEvents(string username, PagingViewModel paging, CancellationToken cancellationToken = default)
        {
            var history = await this.eventStoreRepository.GetEvents(username, paging, cancellationToken);
            var total = await this.eventStoreRepository.Count(username, paging.Search, cancellationToken);
            return new ListOf<EventHistoryData>(this.mapper.Map<IEnumerable<EventHistoryData>>(history), total);
        }

        public Task<bool> HasPassword(string username, CancellationToken cancellationToken = default) => this.userService.HasPassword(username, cancellationToken);

        public async Task<ListOf<UserListViewModel>> SearchUsersByProperties(UserFindByProperties findByProperties, CancellationToken cancellationToken = default)
        {
            var users = this.mapper.Map<IEnumerable<UserListViewModel>>(await this.userService.SearchByProperties(findByProperties.Query, findByProperties, findByProperties, cancellationToken));
            var total = await this.userService.CountByProperties(findByProperties.Query, cancellationToken);
            var claims = await this.GetClaimsFromUsers(users.Select(s => s.UserName), cancellationToken, JwtClaimTypes.Picture, JwtClaimTypes.GivenName);
            foreach (var domainUser in users)
            {
                if (claims.ContainsKey(domainUser.UserName))
                {
                    domainUser.UpdateMetadata(claims[domainUser.UserName]);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return new ListOf<UserListViewModel>(this.mapper.Map<IEnumerable<UserListViewModel>>(users), total);
        }

        public async Task<ListOf<UserListViewModel>> Search(IUserSearch search, CancellationToken cancellationToken = default)
        {
            var users = this.mapper.Map<IEnumerable<UserListViewModel>>(await this.userService.Search(search, cancellationToken));
            var total = await this.userService.Count(search, cancellationToken);
            var claims = await this.GetClaimsFromUsers(users.Select(s => s.UserName), cancellationToken, JwtClaimTypes.Picture, JwtClaimTypes.GivenName);
            foreach (var domainUser in users)
            {
                if (claims.ContainsKey(domainUser.UserName))
                {
                    domainUser.UpdateMetadata(claims[domainUser.UserName]);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return new ListOf<UserListViewModel>(this.mapper.Map<IEnumerable<UserListViewModel>>(users), total);
        }

        private async Task<Dictionary<Username, IEnumerable<Claim>>> GetClaimsFromUsers(IEnumerable<string> usernames, CancellationToken cancellationToken = default, params string[] claimType) => await this.userService.GetClaimsFromUsers(usernames, cancellationToken, claimType);

        public Task<bool> CreatePassword(SetPasswordViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<SetPasswordCommand>(model), cancellationToken);

        public Task<bool> ChangePassword(ChangePasswordViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<ChangePasswordCommand>(model), cancellationToken);

        public Task<bool> UpdateUser(UserViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<AdminUpdateUserCommand>(model), cancellationToken);

        public Task<bool> SaveRole(SaveUserRoleViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<SaveUserRoleCommand>(model), cancellationToken);

        public Task<bool> SaveClaim(SaveUserClaimViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<SaveUserClaimCommand>(model), cancellationToken);

        public Task<bool> UpdateProfile(UserViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<UpdateProfileCommand>(model), cancellationToken);

        public async Task<bool> UpdateProfilePicture(ProfilePictureViewModel model, CancellationToken cancellationToken = default)
        {
            await this.storage.Remove(model.Filename, "images", cancellationToken);
            model.Picture = await this.storage.Upload(model, cancellationToken);
            return await this.bus.SendCommand(this.mapper.Map<UpdateProfilePictureCommand>(model), cancellationToken);
        }

        public Task<bool> RemoveAccount(RemoveAccountViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RemoveAccountCommand>(model), cancellationToken);

        public Task<bool> RemoveClaim(RemoveUserClaimViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RemoveUserClaimCommand>(model), cancellationToken);

        public Task<bool> RemoveRole(RemoveUserRoleViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RemoveUserRoleCommand>(model), cancellationToken);

        public Task<bool> RemoveLogin(RemoveUserLoginViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RemoveUserLoginCommand>(model), cancellationToken);

        public Task<bool> ResetPassword(AdminChangePasswordViewodel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<AdminChangePasswordCommand>(model), cancellationToken);
    }
}
