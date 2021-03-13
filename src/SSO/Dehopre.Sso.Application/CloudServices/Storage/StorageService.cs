namespace Dehopre.Sso.Application.CloudServices.Storage
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.Interfaces;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Domain.ViewModels.Settings;

    public class StorageService : IStorage
    {
        private readonly IGlobalConfigurationAppService globalConfigurationAppService;

        public StorageService(IGlobalConfigurationAppService globalConfigurationAppService) => this.globalConfigurationAppService = globalConfigurationAppService;

        public async Task<string> Upload(FileUploadViewModel image, CancellationToken cancellationToken = default)
        {
            var settings = await this.globalConfigurationAppService.GetPrivateSettings(cancellationToken);
            var provider = GetProvider(settings.Storage.Provider, settings);
            return await provider.Upload(image, cancellationToken);
        }

        public async Task Remove(string filename, string virtualLocation, CancellationToken cancellationToken = default)
        {
            var settings = await this.globalConfigurationAppService.GetPrivateSettings(cancellationToken);
            var provider = GetProvider(settings.Storage.Provider, settings);
            await provider.RemoveFile(filename, virtualLocation, cancellationToken);
        }

        private static IStorageService GetProvider(StorageProviderService storageProvider, PrivateSettings settings) => storageProvider switch
        {
            StorageProviderService.Azure => new AzureStorageService(settings.Storage),
            StorageProviderService.S3 => new AwsStorageService(settings.Storage),
            _ => new LocalStorageService(settings.Storage),
        };
    }
}
