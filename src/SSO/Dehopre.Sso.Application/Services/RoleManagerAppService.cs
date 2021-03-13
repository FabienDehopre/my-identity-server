namespace Dehopre.Sso.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Sso.Application.AutoMapper;
    using Dehopre.Sso.Application.Interfaces;
    using Dehopre.Sso.Application.ViewModels.RoleViewModels;
    using Dehopre.Sso.Domain.Commands.Role;
    using Dehopre.Sso.Domain.Interfaces;
    using global::AutoMapper;

    public class RoleManagerAppService : IRoleManagerAppService
    {
        private readonly IMapper mapper;
        private readonly IRoleService roleService;

        public IMediatorHandler Bus { get; set; }
        public RoleManagerAppService(
            IRoleService roleService,
            IMediatorHandler bus)
        {
            this.mapper = RoleMapping.Mapper;
            this.roleService = roleService;
            this.Bus = bus;
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<IEnumerable<RoleViewModel>> GetAllRoles(CancellationToken cancellationToken = default) => this.mapper.Map<IEnumerable<RoleViewModel>>(await this.roleService.GetAllRoles(cancellationToken));

        public Task Remove(RemoveRoleViewModel model, CancellationToken cancellationToken = default) => this.Bus.SendCommand(this.mapper.Map<RemoveRoleCommand>(model), cancellationToken);

        public async Task<RoleViewModel> GetDetails(string name, CancellationToken cancellationToken = default) => this.mapper.Map<RoleViewModel>(await this.roleService.Details(name, cancellationToken));

        public Task Save(SaveRoleViewModel model, CancellationToken cancellationToken = default) => this.Bus.SendCommand(this.mapper.Map<SaveRoleCommand>(model), cancellationToken);

        public Task Update(string id, UpdateRoleViewModel model, CancellationToken cancellationToken = default) => this.Bus.SendCommand(new UpdateRoleCommand(model.Name, id), cancellationToken);

        public Task RemoveUserFromRole(RemoveUserFromRoleViewModel model, CancellationToken cancellationToken = default) => this.Bus.SendCommand(this.mapper.Map<RemoveUserFromRoleCommand>(model), cancellationToken);
    }
}
