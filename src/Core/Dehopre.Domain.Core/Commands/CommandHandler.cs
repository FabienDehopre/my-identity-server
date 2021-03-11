namespace Dehopre.Domain.Core.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Notifications;
    using MediatR;

    public class CommandHandler
    {
        private readonly IUnitOfWork uow;
        private readonly DomainNotificationHandler notifications;

        public IMediatorHandler Bus { get; }

        public CommandHandler(IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications)
        {
            this.uow = uow;
            this.notifications = (DomainNotificationHandler)notifications;
            this.Bus = bus;
        }

        protected void NotifyValidationErrors(Command message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                this.Bus.RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        public async Task<bool> Commit(CancellationToken cancellationToken = default)
        {
            if (this.notifications.HasNotifications())
            {
                return false;
            }

            if (await this.uow.Commit(cancellationToken))
            {
                return true;
            }

            await this.Bus.RaiseEvent(new DomainNotification("Commit", "We had a problem during saving your data."), cancellationToken);
            return false;
        }
    }
}
