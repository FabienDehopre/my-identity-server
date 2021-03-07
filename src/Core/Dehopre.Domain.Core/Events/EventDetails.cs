namespace Dehopre.Domain.Core.Events
{
    using System;

    public class EventDetails
    {
        protected EventDetails() { }
        public EventDetails(Guid id, string metadata)
        {
            this.EventId = id;
            this.Metadata = metadata;
        }

        public Guid EventId { get; set; }
        public string Metadata { get; set; }
        public StoredEvent Event { get; set; }
    }
}
