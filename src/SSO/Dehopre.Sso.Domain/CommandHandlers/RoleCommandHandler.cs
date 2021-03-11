namespace Dehopre.Sso.Domain.CommandHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Commands;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Notifications;
    using Dehopre.Sso.Domain.Commands.Role;
    using Dehopre.Sso.Domain.Events.Role;
    using Dehopre.Sso.Domain.Interfaces;
    using MediatR;

    public class RoleCommandHandler : CommandHandler,
        IRequestHandler<RemoveRoleCommand, bool>,
        IRequestHandler<SaveRoleCommand, bool>,
        IRequestHandler<UpdateRoleCommand, bool>,
        IRequestHandler<RemoveUserFromRoleCommand, bool>
    {
        private readonly IRoleService roleService;
        private readonly IUserService userService;

        public RoleCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IRoleService roleService,
            IUserService userService) : base(uow, bus, notifications)
        {
            this.roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<bool> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.roleService.Remove(request.Name, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new RoleRemovedEvent(request.Name), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(SaveRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.roleService.Save(request.Name, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new RoleSavedEvent(request.Name), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.roleService.Update(request.Name, request.OldName, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new RoleUpdatedEvent(request.Name, request.OldName), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.RemoveUserFromRole(request.Name, request.Username, cancellationToken);
            if (result)
            {
                await this.Bus.RaiseEvent(new UserRemovedFromRoleEvent(request.Name, request.Username), cancellationToken);
                return true;
            }

            return false;
        }
    }
}
