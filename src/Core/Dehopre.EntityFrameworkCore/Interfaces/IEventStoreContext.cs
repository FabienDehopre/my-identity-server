namespace Dehopre.EntityFrameworkCore.Interfaces
{
    using Dehopre.Domain.Core.Events;
    using Microsoft.EntityFrameworkCore;

    public interface IEventStoreContext : IDehopreEntityFrameworkStore
    {
        public DbSet<StoredEvent> StoredEvent { get; set; }
        public DbSet<EventDetails> StoredEventDetails { get; set; }
    }
}
