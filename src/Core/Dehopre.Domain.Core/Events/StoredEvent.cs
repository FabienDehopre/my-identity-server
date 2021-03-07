namespace Dehopre.Domain.Core.Events
{
    using System;

    public class StoredEvent : Event
    {
        public StoredEvent(string messageType, EventTypes eventType, string customMessage, string localIpAddress, string remoteIpAddress, string data)
            : base(eventType)
        {
            this.Id = Guid.NewGuid();
            this.MessageType = messageType;
            this.EventType = eventType;
            this.Message = customMessage;
            this.LocalIpAddress = localIpAddress;
            this.RemoteIpAddress = remoteIpAddress;
            this.Details = new EventDetails(this.Id, data);
        }

        public StoredEvent SetUser(string user)
        {
            this.User = user;
            return this;
        }

        // EF Constructor
        protected StoredEvent() { }

        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the local ip address of the current request.
        /// </summary>
        /// <value>
        /// The local ip address.
        /// </value>
        public string LocalIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the remote ip address of the current request.
        /// </summary>
        /// <value>
        /// The remote ip address.
        /// </value>
        public string RemoteIpAddress { get; set; }

        public string User { get; private set; }
        public EventDetails Details { get; set; }

        public StoredEvent ReplaceTimeStamp(in DateTime timeStamp)
        {
            this.Timestamp = timeStamp;
            return this;
        }

        public StoredEvent SetAggregate(string aggregateId)
        {
            this.AggregateId = aggregateId;
            return this;
        }
    }
}
