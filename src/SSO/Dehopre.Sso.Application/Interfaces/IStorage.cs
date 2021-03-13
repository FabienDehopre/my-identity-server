namespace Dehopre.Sso.Application.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.ViewModels;

    public interface IStorage
    {
        Task<string> Upload(FileUploadViewModel image, CancellationToken cancellationToken = default);
        Task Remove(string filename, string virtualLocation, CancellationToken cancellationToken = default);
    }
}
