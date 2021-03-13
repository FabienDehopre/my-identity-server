namespace Dehopre.Sso.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.AspNetCore.IQueryable.Extensions;
    using Dehopre.AspNetCore.IQueryable.Extensions.Filter;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.ViewModels;
    using Dehopre.Sso.Application.AutoMapper;
    using Dehopre.Sso.Application.EventSourcedNormalizers;
    using Dehopre.Sso.Application.Interfaces;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using global::AutoMapper;

    public class EventStoreAppService : IEventStoreAppService
    {
        private readonly IEventStoreRepository eventStoreRepository;
        private readonly IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository;
        private readonly IMapper mapper;

        public EventStoreAppService(
            IEventStoreRepository eventStoreRepository,
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository)
        {
            this.mapper = UserMapping.Mapper;
            this.eventStoreRepository = eventStoreRepository;
            this.globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;
        }

        public ListOf<EventHistoryData> GetEvents(ICustomEventQueryable query)
        {
            var history = this.eventStoreRepository.All().Apply((ICustomQueryable)query).ToList();
            var total = this.eventStoreRepository.All().Filter(query).Count();
            if (total > 0)
            {
                return new ListOf<EventHistoryData>(this.mapper.Map<IEnumerable<EventHistoryData>>(history), total);
            }

            return new ListOf<EventHistoryData>(new List<EventHistoryData>(), 0);
        }

        public async Task<IEnumerable<EventSelector>> ListAggregates(CancellationToken cancellationToken = default)
        {
            var selector = new List<EventSelector>
            {
                new EventSelector(AggregateType.Email, EmailType.NewUser.ToString()),
                new EventSelector(AggregateType.Email, EmailType.NewUserWithoutPassword.ToString()),
                new EventSelector(AggregateType.Email, EmailType.RecoverPassword.ToString())
            };

            var keysGlobalConfig = await this.globalConfigurationSettingsRepository.All(cancellationToken);
            selector.AddRange(keysGlobalConfig.Select(s => new EventSelector(AggregateType.GlobalSettings, s.Key)));

            return selector;
        }
    }
}
