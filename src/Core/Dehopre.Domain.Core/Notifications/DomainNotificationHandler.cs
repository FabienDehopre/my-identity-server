namespace Dehopre.Domain.Core.Notifications
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> notifications;

        public DomainNotificationHandler() => this.notifications = new List<DomainNotification>();

        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            this.notifications.Add(message);
            return Task.CompletedTask;
        }
        public virtual Dictionary<string, string[]> GetNotificationsByKey()
        {
            var keys = this.notifications.Select(s => s.Key).Distinct();
            var problemDetails = new Dictionary<string, string[]>();
            foreach (var key in keys)
            {
                problemDetails[key] = this.notifications.Where(w => w.Key.Equals(key)).Select(s => s.Value).ToArray();
            }

            return problemDetails;
        }

        public virtual List<DomainNotification> GetNotifications() => this.notifications;

        public virtual bool HasNotifications() => this.GetNotifications().Any();

        public void Dispose() => this.notifications = new List<DomainNotification>();

        public void Clear() => this.notifications = new List<DomainNotification>();
    }
}
