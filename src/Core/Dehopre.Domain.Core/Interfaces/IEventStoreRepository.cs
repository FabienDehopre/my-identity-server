namespace Dehopre.Domain.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Events;
    using Dehopre.Domain.Core.ViewModels;

    public interface IEventStoreRepository : IDisposable
    {
        Task Store(StoredEvent theEvent);
        IQueryable<StoredEvent> All();
        Task<List<StoredEvent>> GetEvents(string username, PagingViewModel paging);
        Task<int> Count(string username, string search);
    }
}
