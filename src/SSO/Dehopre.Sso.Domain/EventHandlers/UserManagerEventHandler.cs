namespace Dehopre.Sso.Domain.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Domain.Events.UserManagement;
    using MediatR;

    public class UserManagerEventHandler : INotificationHandler<ProfileUpdatedEvent>, INotificationHandler<ProfilePictureUpdatedEvent>, INotificationHandler<PasswordCreatedEvent>, INotificationHandler<PasswordChangedEvent>, INotificationHandler<AccountRemovedEvent>
    {
        // TODO
        public Task Handle(ProfileUpdatedEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;

        // TODO
        public Task Handle(ProfilePictureUpdatedEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;

        // TODO
        public Task Handle(PasswordCreatedEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;

        // TODO
        public Task Handle(PasswordChangedEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;

        // TODO
        public Task Handle(AccountRemovedEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
