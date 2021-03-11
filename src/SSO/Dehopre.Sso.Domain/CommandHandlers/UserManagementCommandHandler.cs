namespace Dehopre.Sso.Domain.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Commands;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Notifications;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using Dehopre.Sso.Domain.Events.User;
    using Dehopre.Sso.Domain.Events.UserManagement;
    using Dehopre.Sso.Domain.Interfaces;
    using MediatR;

    public class UserManagementCommandHandler : CommandHandler,
        IRequestHandler<UpdateProfileCommand, bool>,
        IRequestHandler<UpdateProfilePictureCommand, bool>,
        IRequestHandler<SetPasswordCommand, bool>,
        IRequestHandler<ChangePasswordCommand, bool>,
        IRequestHandler<RemoveAccountCommand, bool>,
        IRequestHandler<AdminUpdateUserCommand, bool>,
        IRequestHandler<SaveUserClaimCommand, bool>,
        IRequestHandler<RemoveUserClaimCommand, bool>,
        IRequestHandler<RemoveUserRoleCommand, bool>,
        IRequestHandler<SaveUserRoleCommand, bool>,
        IRequestHandler<RemoveUserLoginCommand, bool>,
        IRequestHandler<AdminChangePasswordCommand, bool>,
        IRequestHandler<SynchronizeClaimsCommand, bool>
    {
        private readonly IUserService userService;
        private readonly ISystemUser user;

        public UserManagementCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IUserService userService,
            ISystemUser user) : base(uow, bus, notifications)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.UpdateProfileAsync(request, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new ProfileUpdatedEvent(request.Username, request), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.UpdateProfilePictureAsync(request, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new ProfilePictureUpdatedEvent(request.Username, request.Picture), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.CreatePasswordAsync(request, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new PasswordCreatedEvent(request.Username), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.ChangePasswordAsync(request, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new PasswordChangedEvent(request.Username), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.RemoveAccountAsync(request, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new AccountRemovedEvent(request.Username), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(AdminUpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (user is null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Username", "User not found."), cancellationToken);
                return false;
            }

            _ = await this.userService.UpdateUserAsync(request, cancellationToken);
            return true;
        }

        public async Task<bool> Handle(SaveUserClaimCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (user is null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Username", "User not found."), cancellationToken);
                return false;
            }

            var claim = new Claim(request.Type, request.Value);
            var success = await this.userService.SaveClaim(user.UserName, claim, cancellationToken);
            if (success)
            {
                await this.Bus.RaiseEvent(new NewUserClaimEvent(request.Username, request.Type, request.Value), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(RemoveUserClaimCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (user is null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Username", "User not found."), cancellationToken);
                return false;
            }

            var success = await this.userService.RemoveClaim(user.UserName, request.Type, request.Value, cancellationToken);
            if (success)
            {
                await this.Bus.RaiseEvent(new UserClaimRemovedEvent(request.Username, request.Type), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (user is null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Username", "User not found."), cancellationToken);
                return false;
            }

            var success = await this.userService.RemoveRole(user.UserName, request.Role, cancellationToken);
            if (success)
            {
                await this.Bus.RaiseEvent(new UserRoleRemovedEvent(this.user.Username, request.Role), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(SaveUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (user is null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Username", "User not found."), cancellationToken);
                return false;
            }

            var success = await this.userService.SaveRole(user.UserName, request.Role, cancellationToken);
            if (success)
            {
                await this.Bus.RaiseEvent(new UserRoleSavedEvent(this.user.Username, request.Role), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(RemoveUserLoginCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (user is null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Username", "User not found."), cancellationToken);
                return false;
            }

            var success = await this.userService.RemoveLogin(user.UserName, request.LoginProvider, request.ProviderKey, cancellationToken);
            if (success)
            {
                await this.Bus.RaiseEvent(new UserLoginRemovedEvent(this.user.Username, request.LoginProvider, request.ProviderKey), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(AdminChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (user is null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Username", "User not found."), cancellationToken);
                return false;
            }

            var success = await this.userService.ResetPasswordAsync(user.UserName, request.Password, cancellationToken);
            if (success)
            {
                await this.Bus.RaiseEvent(new AdminChangedPasswordEvent(user.UserName), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(SynchronizeClaimsCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var userClaims = (await this.userService.GetClaimByName(request.Username, cancellationToken)).ToList();
            foreach (var claim in request.Claims)
            {
                var actualUserClaim = userClaims.Find(f => f.Type == claim.Type);
                if (actualUserClaim is null)
                {
                    _ = await this.userService.SaveClaim(request.Username, claim, cancellationToken);
                }
                else
                {
                    var newValue = claim.Value;
                    var currentValue = actualUserClaim.Value;
                    if (currentValue != newValue)
                    {
                        _ = await this.userService.RemoveClaim(request.Username, actualUserClaim.Type, actualUserClaim.Value, cancellationToken);
                        _ = await this.userService.SaveClaim(request.Username, claim, cancellationToken);
                    }
                }
            }

            if (await this.Commit(cancellationToken))
            {
                await this.Bus.RaiseEvent(new ClaimsSyncronizedEvent(request.Username, request.Claims), cancellationToken);
                return true;
            }

            return false;
        }
    }
}
