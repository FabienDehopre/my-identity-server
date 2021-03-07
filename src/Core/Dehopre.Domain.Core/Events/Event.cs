namespace Dehopre.Domain.Core.Events
{
    using System;
    using MediatR;

    public abstract class Event : Message, INotification
    {
        /// <summary>
        /// Gets or sets the time stamp when the event was raised.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public DateTime Timestamp { get; protected set; }

        public EventTypes EventType { get; set; }

        /// <summary>
        /// Gets or sets the event message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        protected Event() { }
        protected Event(EventTypes eventType)
        {
            this.EventType = eventType;
            this.Timestamp = DateTime.UtcNow;
        }
    }
}
