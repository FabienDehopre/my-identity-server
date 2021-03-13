namespace Dehopre.Sso.Application.CloudServices.Storage
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Util;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Domain.ViewModels.Settings;

    public class LocalStorageService : IStorageService
    {
        private readonly StorageSettings privateSettings;

        public LocalStorageService(StorageSettings privateSettings) => this.privateSettings = privateSettings;

        public Task RemoveFile(string fileName, string virtualLocation, CancellationToken cancellationToken = default)
        {
            var directory = Path.Combine(this.privateSettings.PhysicalPath, this.privateSettings.VirtualPath ?? string.Empty, virtualLocation ?? string.Empty);
            if (!Directory.Exists(directory))
            {
                _ = Directory.CreateDirectory(directory);
            }

            var file = Path.Combine(directory, fileName);
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            return Task.CompletedTask;
        }

        public async Task<string> Upload(FileUploadViewModel image, CancellationToken cancellationToken = default)
        {
            var bytes = Convert.FromBase64String(image.Value);
            var path = Path.Combine(this.privateSettings.PhysicalPath, this.privateSettings.VirtualPath ?? string.Empty, image.VirtualLocation ?? string.Empty, image.Filename);

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            await File.WriteAllBytesAsync(path, bytes, cancellationToken);


            return this.privateSettings.BasePath.UrlPathCombine(this.privateSettings.VirtualPath ?? string.Empty, image.VirtualLocation ?? string.Empty, image.Filename);
        }
    }
}
