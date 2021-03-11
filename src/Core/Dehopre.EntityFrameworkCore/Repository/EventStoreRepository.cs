namespace Dehopre.EntityFrameworkCore.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Events;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Util;
    using Dehopre.Domain.Core.ViewModels;
    using Dehopre.EntityFrameworkCore.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IEventStoreContext context;

        public EventStoreRepository(IEventStoreContext context) => this.context = context ?? throw new ArgumentNullException(nameof(context));

        public IQueryable<StoredEvent> All() => this.context.StoredEvent.Include(s => s.Details);

        public async Task<List<StoredEvent>> GetEvents(string username, PagingViewModel paging, CancellationToken cancellationToken = default)
        {
            List<StoredEvent> events;
            if (paging.Search.IsPresent())
            {
                events = await this.context.StoredEvent
                                    .Include(s => s.Details)
                                    .Where(EventFind(username, paging.Search))
                                    .OrderByDescending(o => o.Timestamp)
                                    .Skip(paging.Offset)
                                    .Take(paging.Limit)
                                    .ToListAsync(cancellationToken);
            }
            else
            {
                events = await this.context.StoredEvent
                                    .Include(s => s.Details)
                                    .Where(w => w.User == username)
                                    .Skip(paging.Offset)
                                    .OrderByDescending(o => o.Timestamp)
                                    .Take(paging.Limit)
                                    .ToListAsync(cancellationToken);
            }

            return events;
        }

        private static Expression<Func<StoredEvent, bool>> EventFind(string username, string search) => w => (w.Message.Contains(search) ||
                                                                                                                              w.MessageType.Contains(search) ||
                                                                                                                              w.AggregateId.Contains(search)) &&
                                                                                                                              w.User == username;

        public Task<int> Count(string username, string search, CancellationToken cancellationToken = default) => search.IsPresent() ? this.context.StoredEvent.Where(EventFind(username, search)).CountAsync(cancellationToken) : this.context.StoredEvent.Where(w => w.User == username).CountAsync(cancellationToken);

        public async Task Store(StoredEvent theEvent, CancellationToken cancellationToken = default)
        {
            _ = await this.context.StoredEvent.AddAsync(theEvent, cancellationToken);
            _ = await this.context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.context.Dispose();
        }
    }
}
