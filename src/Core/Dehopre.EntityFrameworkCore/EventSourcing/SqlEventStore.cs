namespace Dehopre.EntityFrameworkCore.EventSourcing
{
    using System;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Events;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Util;

    public class SqlEventStore : IEventStore
    {
        private readonly IEventStoreRepository eventStoreRepository;
        private readonly ISystemUser systemUser;

        public SqlEventStore(IEventStoreRepository eventStoreRepository, ISystemUser systemUser)
        {
            this.eventStoreRepository = eventStoreRepository ?? throw new ArgumentNullException(nameof(eventStoreRepository));
            this.systemUser = systemUser ?? throw new ArgumentNullException(nameof(systemUser));
        }

        public Task Save<T>(T theEvent) where T : Event
        {
            var serializedData = theEvent.ToJson();
            if (theEvent.Message.IsMissing())
            {
                theEvent.Message = theEvent.MessageType.AddSpacesToSentence().Replace("Event", string.Empty).Trim();
            }

            var storedEvent = new StoredEvent(theEvent.MessageType, theEvent.EventType, theEvent.Message, this.systemUser.GetLocalIpAddress(), this.systemUser.GetRemoteIpAddress(), serializedData)
                .SetUser(this.systemUser.Username)
                .SetAggregate(theEvent.AggregateId);
            return this.eventStoreRepository.Store(storedEvent);
        }
    }
}
