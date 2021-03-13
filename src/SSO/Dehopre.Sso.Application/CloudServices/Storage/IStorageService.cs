namespace Dehopre.Sso.Application.CloudServices.Storage
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.ViewModels;

    public interface IStorageService
    {
        Task<string> Upload(FileUploadViewModel file, CancellationToken cancellationToken = default);
        Task RemoveFile(string fileName, string virtualLocation, CancellationToken cancellationToken = default);
    }
}
