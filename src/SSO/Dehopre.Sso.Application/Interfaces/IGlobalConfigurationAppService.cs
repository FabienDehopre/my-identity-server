namespace Dehopre.Sso.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Domain.Interfaces;

    public interface IGlobalConfigurationAppService : IGlobalConfigurationSettingsService
    {
        Task<bool> UpdateSettings(IEnumerable<ConfigurationViewModel> configs, CancellationToken cancellationToken = default);
        Task<IEnumerable<ConfigurationViewModel>> ListSettings(CancellationToken cancellationToken = default);
    }
}
