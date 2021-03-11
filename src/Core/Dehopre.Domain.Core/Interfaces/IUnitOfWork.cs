namespace Dehopre.Domain.Core.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit(CancellationToken cancellationToken = default);
    }
}
