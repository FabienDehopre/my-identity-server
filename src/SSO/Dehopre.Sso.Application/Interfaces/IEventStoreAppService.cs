namespace Dehopre.Sso.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.ViewModels;
    using Dehopre.Sso.Application.EventSourcedNormalizers;

    public interface IEventStoreAppService
    {
        ListOf<EventHistoryData> GetEvents(ICustomEventQueryable query);
        Task<IEnumerable<EventSelector>> ListAggregates(CancellationToken cancellationToken = default);
    }
}
