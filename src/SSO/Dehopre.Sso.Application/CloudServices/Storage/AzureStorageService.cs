namespace Dehopre.Sso.Application.CloudServices.Storage
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Domain.ViewModels.Settings;

    public class AzureStorageService : IStorageService
    {
        private readonly StorageSettings privateSettings;

        public AzureStorageService(StorageSettings privateSettings) => this.privateSettings = privateSettings ?? throw new ArgumentNullException(nameof(privateSettings));

        public async Task RemoveFile(string fileName, string virtualLocation, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<string> Upload(FileUploadViewModel file, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
