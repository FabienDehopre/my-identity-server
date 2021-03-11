namespace Dehopre.Sso.Domain.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Domain.Events.User;
    using MediatR;

    public class UserEventHandler : INotificationHandler<UserRegisteredEvent>, INotificationHandler<EmailConfirmedEvent>
    {
        // TODO
        public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;

        // TODO
        public Task Handle(EmailConfirmedEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
