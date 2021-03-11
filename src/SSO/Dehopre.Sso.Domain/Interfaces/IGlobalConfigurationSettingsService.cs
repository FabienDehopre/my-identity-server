namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Threading.Tasks;
    using Dehopre.Sso.Domain.ViewModels.Settings;

    public interface IGlobalConfigurationSettingsService
    {
        Task<PrivateSettings> GetPrivateSettings();
        Task<PublicSettings> GetPublicSettings();
    }
}
