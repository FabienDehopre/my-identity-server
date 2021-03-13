namespace Dehopre.Sso.Application.AutoMapper
{
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Domain.Commands.GlobalConfiguration;
    using Dehopre.Sso.Domain.Models;
    using global::AutoMapper;

    public class GlobalConfigurationMapperProfile : Profile
    {
        public GlobalConfigurationMapperProfile()
        {

            /*
             * Global configuration commands
             */
            _ = this.CreateMap<ConfigurationViewModel, ManageConfigurationCommand>().ConstructUsing(c => new ManageConfigurationCommand(c.Key, c.Value, c.IsSensitive, c.IsPublic));

            _ = this.CreateMap<GlobalConfigurationSettings, ConfigurationViewModel>(MemberList.Destination).ForMember(m => m.IsSensitive, opt => opt.MapFrom(src => src.Sensitive));

        }
    }
}
