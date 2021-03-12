namespace Dehopre.Sso.AspNetIdentity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.AspNetCore.IQueryable.Extensions;
    using Dehopre.AspNetCore.IQueryable.Extensions.Filter;
    using Dehopre.AspNetCore.IQueryable.Extensions.Pagination;
    using Dehopre.AspNetCore.IQueryable.Extensions.Sort;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Notifications;
    using Dehopre.Domain.Core.Util;
    using Dehopre.EntityFrameworkCore.Interfaces;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using Dehopre.Sso.Domain.ViewModels;
    using Dehopre.Sso.Domain.ViewModels.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class UserService<TUser, TRole, TKey> : IUserService
        where TUser : IdentityUser<TKey>, IDomainUser
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> userManager;
        private readonly IMediatorHandler bus;
        private readonly ILogger logger;
        private readonly IConfiguration config;
        private readonly IIdentityFactory<TUser> userFactory;
        private readonly IOptions<IdentityOptions> identityOptions;
        private readonly IDehopreEntityFrameworkStore store;

        public UserService(
            UserManager<TUser> userManager,
            IMediatorHandler bus,
            ILoggerFactory loggerFactory,
            IConfiguration config,
            IIdentityFactory<TUser> userFactory,
            IOptions<IdentityOptions> identityOptions,
            IDehopreEntityFrameworkStore store)
        {
            this.userManager = userManager;
            this.bus = bus;
            this.config = config;
            this.userFactory = userFactory;
            this.identityOptions = identityOptions;
            this.store = store;

            this.logger = loggerFactory.CreateLogger<UserService<TUser, TRole, TKey>>();
        }

        public Task<AccountResult?> CreateUserWithPass(RegisterNewUserCommand command, string password, CancellationToken cancellationToken = default)
        {
            var user = this.userFactory.Create(command);
            return this.CreateUser(user, command, password, null, null, cancellationToken);
        }

        public Task<AccountResult?> CreateUserWithouthPassword(RegisterNewUserWithoutPassCommand command, CancellationToken cancellationToken = default)
        {
            var user = this.userFactory.Create(command);
            return this.CreateUser(user, command, null, command.Provider, command.ProviderId, cancellationToken);
        }

        public Task<AccountResult?> CreateUserWithProviderAndPass(RegisterNewUserWithProviderCommand command, CancellationToken cancellationToken = default)
        {
            var user = this.userFactory.Create(command);
            return this.CreateUser(user, command, command.Password, command.Provider, command.ProviderId, cancellationToken);
        }

        public async Task<bool> UsernameExist(string userName, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            return user != null;
        }

        public async Task<bool> EmailExist(string email, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            return user != null;
        }


        public async Task<AccountResult?> GenerateResetPasswordLink(string emailOrUsername, CancellationToken cancellationToken = default)
        {
            var user = await this.GetUserByEmailOrUsername(emailOrUsername);
            if (user == null)
            {
                return null;
            }


            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"{this.config.GetValue<string>("ApplicationSettings:UserManagementURL")}/reset-password?email={user.Email.UrlEncode()}&code={code.UrlEncode()}";

            //await _emailService.SendEmailAsync(user.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            //_logger.LogInformation("Reset link sended to userId.");

            return new AccountResult(user.UserName, code, callbackUrl);
        }

        public async Task<string> ConfirmEmailAsync(string email, string code, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                await this.bus.RaiseEvent(new DomainNotification("Email", $"Unable to load userId with ID '{email}'."), cancellationToken);
                return null;
            }

            var result = await this.userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return user.UserName;
            }

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return null;
        }



        public async Task<bool> UpdateProfileAsync(UpdateProfileCommand command, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(command.Username);
            this.userFactory.UpdateProfile(command, user);

            var claims = await this.userManager.GetClaimsAsync(user);

            _ = await this.AddOrUpdateClaim(claims, user, JwtClaimTypes.GivenName, command.Name);
            _ = await this.AddOrUpdateClaim(claims, user, JwtClaimTypes.WebSite, command.Url);

            if (command.Birthdate.HasValue)
            {
                _ = await this.AddOrUpdateClaim(claims, user, JwtClaimTypes.BirthDate, command.Birthdate.Value.ToString(CultureInfo.CurrentCulture));
            }

            _ = await this.AddOrUpdateClaim(claims, user, "company", command.Company);
            _ = await this.AddOrUpdateClaim(claims, user, "job_title", command.JobTitle);
            _ = await this.AddOrUpdateClaim(claims, user, "bio", command.Bio);
            _ = await this.AddOrUpdateClaim(claims, user, "social_number", command.SocialNumber);

            var result = await this.userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return false;
        }

        public async Task<bool> UpdateProfilePictureAsync(UpdateProfilePictureCommand command, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(command.Username);
            var claims = await this.userManager.GetClaimsAsync(user);

            IdentityResult result;
            var pictureClaim = claims.Of(JwtClaimTypes.Picture);
            var newPictureClaim = new Claim(JwtClaimTypes.Picture, command.Picture);
            if (pictureClaim != null)
            {
                result = await this.userManager.ReplaceClaimAsync(user, pictureClaim, newPictureClaim);
            }
            else
            {
                result = await this.userManager.AddClaimAsync(user, newPictureClaim);
            }

            if (result.Succeeded)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return false;
        }


        public async Task<bool> UpdateUserAsync(AdminUpdateUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(command.Username);
            this.userFactory.UpdateInfo(command, user);

            var claims = await this.userManager.GetClaimsAsync(user);

            if (command.Name.IsPresent())
            {
                _ = await this.AddOrUpdateClaim(claims, user, JwtClaimTypes.GivenName, command.Name);
            }

            if (command.Birthdate.HasValue)
            {
                _ = await this.AddOrUpdateClaim(claims, user, JwtClaimTypes.BirthDate, command.Birthdate.Value.ToString(CultureInfo.CurrentCulture));
            }

            if (command.SocialNumber.IsPresent())
            {
                _ = await this.AddOrUpdateClaim(claims, user, "social_number", command.SocialNumber);
            }

            var resut = await this.userManager.UpdateAsync(user);
            if (!resut.Succeeded)
            {
                foreach (var error in resut.Errors)
                {
                    await this.bus.RaiseEvent(new DomainNotification("User", error.Description), cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }

                return false;
            }

            return true;
        }

        public async Task<bool> CreatePasswordAsync(SetPasswordCommand request, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(request.Username);

            var hasPassword = await this.userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                /*
                 * DO NOT display the reason.
                 * if this happen is because userId are trying to hack.
                 */
                throw new Exception("Unknown error");
            }

            var result = await this.userManager.AddPasswordAsync(user, request.Password);
            if (result.Succeeded)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return false;
        }

        public async Task<bool> RemoveAccountAsync(RemoveAccountCommand request, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(request.Username);
            var result = await this.userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return false;
        }

        public async Task<bool> HasPassword(string username, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);

            return await this.userManager.HasPasswordAsync(user);
        }

        public async Task<IDomainUser> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (email.IsMissing())
            {
                return null;
            }

            var user = await this.userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<IDomainUser> FindByNameAsync(string username, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);
            return user;
        }

        public async Task<IDomainUser> FindByProviderAsync(string provider, string providerUserId, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByLoginAsync(provider, providerUserId);
            return user;
        }

        public async Task<IEnumerable<Claim>> GetClaimByName(string userName, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            var claims = await this.userManager.GetClaimsAsync(user);

            return claims;
        }

        public async Task<bool> SaveClaim(string username, Claim claim, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var result = await this.userManager.AddClaimAsync(user, claim);

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> RemoveClaim(string username, string claimType, string value, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var claims = await this.userManager.GetClaimsAsync(user);

            var claimToRemove = value.IsMissing() ?
                                    claims.First(c => c.Type.Equals(claimType)) :
                                    claims.First(c => c.Type.Equals(claimType) && c.Value.Equals(value));

            var result = await this.userManager.RemoveClaimAsync(user, claimToRemove);

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetRoles(string userName, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            return await this.userManager.GetRolesAsync(user);
        }

        public async Task<bool> RemoveRole(string username, string requestRole, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var result = await this.userManager.RemoveFromRoleAsync(user, requestRole);

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }


        public async Task<bool> SaveRole(string username, string role, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var result = await this.userManager.AddToRoleAsync(user, role);

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }

        public async Task<IEnumerable<UserLogin>> GetUserLogins(string userName, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            var logins = await this.userManager.GetLoginsAsync(user);
            return logins.Select(a => new UserLogin(a.LoginProvider, a.ProviderDisplayName, a.ProviderKey));
        }

        public async Task<bool> RemoveLogin(string username, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var result = await this.userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }

        public async Task<IEnumerable<IDomainUser>> GetUserFromRole(string role, CancellationToken cancellationToken = default) => await this.userManager.GetUsersInRoleAsync(role);

        public async Task<bool> RemoveUserFromRole(string name, string username, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var result = await this.userManager.RemoveFromRoleAsync(user, name);
            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(username);
            _ = await this.userManager.RemovePasswordAsync(user);
            var result = await this.userManager.AddPasswordAsync(user, password);
            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }

        public async Task<string> ResetPassword(string email, string code, string password, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal that the userId does not exist
                return null;
            }

            var result = await this.userManager.ResetPasswordAsync(user, code, password);

            if (result.Succeeded)
            {
                var emailConfirmed = await this.userManager.IsEmailConfirmedAsync(user);
                if (!emailConfirmed)
                {
                    user.ConfirmEmail();
                    _ = await this.userManager.UpdateAsync(user);

                }

                this.logger.LogInformation("Password reseted successfull.");
                return user.UserName;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            return null;
        }


        public async Task<bool> ChangePasswordAsync(ChangePasswordCommand request, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByNameAsync(request.Username);
            var result = await this.userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);
            if (result.Succeeded)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return false;
        }
        /// <summary>
        /// Add login for user, if success return his username
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="provider">provider: eg, Google, Facebook</param>
        /// <param name="providerId">Unique identifier from provider</param>
        /// <returns></returns>
        public async Task<string> AddLoginAsync(string email, string provider, string providerId, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            await this.AddLoginAsync(user, provider, providerId, cancellationToken);

            return user.UserName;
        }

        public async Task<IDomainUser> FindByUsernameOrEmail(string emailOrUsername, CancellationToken cancellationToken = default) => await this.GetUserByEmailOrUsername(emailOrUsername);

        public async Task<int> CountByProperties(string query, CancellationToken cancellationToken = default) => await (from user in this.store.Set<TUser>().AsQueryable()
                                                                                                                        join claim in this.store.Set<IdentityUserClaim<TKey>>().AsQueryable() on user.Id equals claim.UserId into userClaims
                                                                                                                        from c in userClaims.DefaultIfEmpty()
                                                                                                                        where
                                                                                                                            user.UserName.Contains(query) ||
                                                                                                                             user.Email.Contains(query) ||
                                                                                                                             c.ClaimValue.Contains(query) ||
                                                                                                                            query == null
                                                                                                                        select
                                                                                                                            user.UserName)
                          .Distinct()
                          .CountAsync(cancellationToken);

        public async Task<IEnumerable<IDomainUser>> SearchByProperties(string query, IQuerySort sort, IQueryPaging paging, CancellationToken cancellationToken = default)
        {
            var usersId = await (
                    from user in this.store.Set<TUser>().AsQueryable()
                    join claim in this.store.Set<IdentityUserClaim<TKey>>().AsQueryable() on user.Id equals claim.UserId into userClaims
                    from c in userClaims.DefaultIfEmpty()
                    where
                        user.UserName.Contains(query) ||
                         user.Email.Contains(query) ||
                         c.ClaimValue.Contains(query) ||
                         query == null
                    group user by user.Id)
                .Sort(sort)
                .Paginate(paging)
                .Select(s => s.Key)
                .ToListAsync(cancellationToken);

            return this.userManager.Users.Where(w => usersId.Contains(w.Id));
        }
        public Task<int> Count(ICustomQueryable search, CancellationToken cancellationToken = default) => this.userManager.Users.Filter(search).CountAsync(cancellationToken);

        public async Task<IEnumerable<IDomainUser>> Search(ICustomQueryable search, CancellationToken cancellationToken = default) => await this.userManager.Users.Apply(search).ToListAsync(cancellationToken);

        public async Task<Dictionary<Username, IEnumerable<Claim>>> GetClaimsFromUsers(IEnumerable<string> username, CancellationToken cancellationToken = default, params string[] claimType)
        {
            var claims = await (from claim in this.store.Set<IdentityUserClaim<TKey>>().AsQueryable()
                                join user in this.store.Set<TUser>().AsQueryable() on claim.UserId equals user.Id
                                where username.Contains(user.UserName) && ((claimType != null && claimType.Contains(claim.ClaimType)) || claimType == null)
                                select new { user.UserName, claim }).ToListAsync(cancellationToken);

            var dictionary = new Dictionary<Username, IEnumerable<Claim>>();

            foreach (var user in username)
            {
                dictionary.Add(user, claims.Where(w => w.UserName == user).Select(s => s.claim.ToClaim()));
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return dictionary;
        }

        public Task<Dictionary<Username, IEnumerable<Claim>>> GetClaimsFromUsers(IEnumerable<string> username, params string[] claimType) => this.GetClaimsFromUsers(username, new CancellationToken(false), claimType);

        private async Task<AccountResult?> CreateUser(TUser user, UserCommand command, string password, string provider, string providerId, CancellationToken cancellationToken)
        {
            if (provider.IsPresent())
            {
                var userByProvider = await this.userManager.FindByLoginAsync(provider, providerId);
                if (userByProvider != null)
                {
                    await this.bus.RaiseEvent(new DomainNotification("New User", $"User already taken with {provider}"), cancellationToken);
                }
            }

            IdentityResult result;
            if (password.IsMissing())
            {
                result = await this.userManager.CreateAsync(user);
            }
            else
            {
                result = await this.userManager.CreateAsync(user, password);
            }

            if (result.Succeeded)
            {
                // User claim for write customers data
                //await _userManager.AddClaimAsync(newUser, new Claim("User", "Write"));
                AccountResult account;
                if (this.identityOptions.Value.User.RequireUniqueEmail && user.Email.IsEmail())
                {
                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = $"{this.config.GetValue<string>("ApplicationSettings:UserManagementURL")}/confirm-email?userId={user.Email.UrlEncode()}&code={code.UrlEncode()}";
                    account = new AccountResult(user.UserName, code, callbackUrl);
                }
                else
                {
                    account = new AccountResult(user.UserName);
                }

                await this.AddClaims(user, command, cancellationToken);

                if (!string.IsNullOrWhiteSpace(provider))
                {
                    await this.AddLoginAsync(user, provider, providerId, cancellationToken);
                }

                if (password.IsPresent())
                {
                    this.logger.LogInformation("User created a new account with password.");
                }

                if (provider.IsPresent())
                {
                    this.logger.LogInformation($"Provider {provider} associated.");
                }

                return account;
            }

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return null;
        }

        /// <summary>
        /// Add custom claims here
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task AddClaims(TUser user, UserCommand command, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Begin include claims");
            var claims = new List<Claim>();
            this.logger.LogInformation(command.ToJson());
            if (command.Picture.IsPresent())
            {
                claims.Add(new Claim(JwtClaimTypes.Picture, command.Picture));
            }

            if (command.Name.IsPresent())
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, command.Name));
            }

            if (command.Birthdate.HasValue)
            {
                claims.Add(new Claim(JwtClaimTypes.BirthDate, command.Birthdate.Value.ToString(CultureInfo.CurrentCulture)));
            }

            if (command.SocialNumber.IsPresent())
            {
                claims.Add(new Claim("social_number", command.SocialNumber));
            }

            if (claims.Any())
            {
                var result = await this.userManager.AddClaimsAsync(user, claims);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("Claim created successfull.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private async Task<IdentityResult> AddOrUpdateClaim(IEnumerable<Claim> claims, TUser user, string type, string value)
        {
            var pictureClaim = claims.Of(type);
            var newPictureClaim = new Claim(type, value);
            if (pictureClaim != null)
            {
                return await this.userManager.ReplaceClaimAsync(user, pictureClaim, newPictureClaim);
            }
            else
            {
                return await this.userManager.AddClaimAsync(user, newPictureClaim);
            }
        }


        private async Task AddLoginAsync(TUser user, string provider, string providerUserId, CancellationToken cancellationToken)
        {
            var result = await this.userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));

            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private async Task<TUser> GetUserByEmailOrUsername(string emailOrUsername)
        {
            TUser user;
            if (emailOrUsername.IsEmail())
            {
                user = await this.userManager.FindByEmailAsync(emailOrUsername);
            }
            else
            {
                user = await this.userManager.FindByNameAsync(emailOrUsername);
            }

            return user;
        }
    }
}
