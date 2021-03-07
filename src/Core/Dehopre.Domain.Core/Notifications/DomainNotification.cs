namespace Dehopre.Domain.Core.Notifications
{
    using System;
    using Dehopre.Domain.Core.Events;

    public class DomainNotification : Event
    {
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }

        public DomainNotification(string key, string value)
            : base(EventTypes.Failure)
        {
            this.DomainNotificationId = Guid.NewGuid();
            this.Version = 1;
            this.Key = key;
            this.Value = value;
        }
    }
}
