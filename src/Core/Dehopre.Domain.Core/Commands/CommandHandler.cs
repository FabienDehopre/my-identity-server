namespace Dehopre.Domain.Core.Commands
{
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Notifications;
    using MediatR;

    public class CommandHandler
    {
        private readonly IUnitOfWork uow;
        public readonly IMediatorHandler ____RULE_VIOLATION____Bus____RULE_VIOLATION____;
        private readonly DomainNotificationHandler notifications;

        public CommandHandler(IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications)
        {
            this.uow = uow;
            this.notifications = (DomainNotificationHandler)notifications;
            this.____RULE_VIOLATION____Bus____RULE_VIOLATION____ = bus;
        }

        protected void NotifyValidationErrors(Command message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                this.____RULE_VIOLATION____Bus____RULE_VIOLATION____.RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        public async Task<bool> Commit()
        {
            if (this.notifications.HasNotifications())
            {
                return false;
            }

            if (await this.uow.Commit())
            {
                return true;
            }

            await this.____RULE_VIOLATION____Bus____RULE_VIOLATION____.RaiseEvent(new DomainNotification("Commit", "We had a problem during saving your data."));
            return false;
        }
    }
}
