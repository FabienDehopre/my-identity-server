namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Sso.Domain.Models;

    public interface IGlobalConfigurationSettingsRepository : IRepository<GlobalConfigurationSettings>
    {
        Task<GlobalConfigurationSettings> FindByKey(string key);
        Task<List<GlobalConfigurationSettings>> All();
    }
}
