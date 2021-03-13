namespace Dehopre.Sso.Application.AutoMapper
{
    using Dehopre.Sso.Application.ViewModels.RoleViewModels;
    using Dehopre.Sso.Domain.Commands.Role;
    using Dehopre.Sso.Domain.Models;
    using global::AutoMapper;

    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            /*
              * Role commands
              */
            _ = this.CreateMap<RemoveRoleViewModel, RemoveRoleCommand>().ConstructUsing(c => new RemoveRoleCommand(c.Name));
            _ = this.CreateMap<SaveRoleViewModel, SaveRoleCommand>().ConstructUsing(c => new SaveRoleCommand(c.Name));
            _ = this.CreateMap<RemoveUserFromRoleViewModel, RemoveUserFromRoleCommand>().ConstructUsing(c => new RemoveUserFromRoleCommand(c.Role, c.Username));

            /*
             * Domain to view model
             */
            _ = this.CreateMap<Role, RoleViewModel>(MemberList.Destination);
        }
    }
}
