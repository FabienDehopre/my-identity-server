namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Sso.Domain.Models;

    public interface IGlobalConfigurationSettingsRepository : IRepository<GlobalConfigurationSettings>
    {
        Task<GlobalConfigurationSettings> FindByKey(string key, CancellationToken cancellationToken = default);
        Task<List<GlobalConfigurationSettings>> All(CancellationToken cancellationToken = default);
    }
}
