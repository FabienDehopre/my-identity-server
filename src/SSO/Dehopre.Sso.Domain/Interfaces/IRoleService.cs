namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Domain.Models;

    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken = default);
        Task<bool> Remove(string name, CancellationToken cancellationToken = default);
        Task<Role> Details(string name, CancellationToken cancellationToken = default);
        Task<bool> Save(string name, CancellationToken cancellationToken = default);
        Task<bool> Update(string name, string oldName, CancellationToken cancellationToken = default);
    }
}
