namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Domain.ViewModels.Settings;

    public interface IGlobalConfigurationSettingsService
    {
        Task<PrivateSettings> GetPrivateSettings(CancellationToken cancellationToken = default);
        Task<PublicSettings> GetPublicSettings(CancellationToken cancellationToken = default);
    }
}
