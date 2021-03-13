namespace Dehopre.Sso.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.ViewModels.RoleViewModels;

    public interface IRoleManagerAppService : IDisposable
    {
        Task<IEnumerable<RoleViewModel>> GetAllRoles(CancellationToken cancellationToken = default);
        Task Remove(RemoveRoleViewModel model, CancellationToken cancellationToken = default);
        Task<RoleViewModel> GetDetails(string name, CancellationToken cancellationToken = default);
        Task Save(SaveRoleViewModel model, CancellationToken cancellationToken = default);
        Task Update(string id, UpdateRoleViewModel model, CancellationToken cancellationToken = default);
        Task RemoveUserFromRole(RemoveUserFromRoleViewModel model, CancellationToken cancellationToken = default);
    }
}
