namespace Dehopre.Sso.Application.AutoMapper
{
    using Dehopre.Sso.Application.ViewModels.EmailViewModels;
    using Dehopre.Sso.Domain.Commands.Email;
    using Dehopre.Sso.Domain.Models;
    using global::AutoMapper;

    public class EmailMapperProfile : Profile
    {
        public EmailMapperProfile()
        {
            /*
             * Email commands
             */
            _ = this.CreateMap<EmailViewModel, SaveEmailCommand>().ConstructUsing(c => new SaveEmailCommand(c.Content, c.Sender, c.Subject, c.Type, c.Bcc, c.Username));
            _ = this.CreateMap<TemplateViewModel, SaveTemplateCommand>().ConstructUsing(c => new SaveTemplateCommand(c.Subject, c.Content, c.Name, c.Username));
            _ = this.CreateMap<TemplateViewModel, UpdateTemplateCommand>().ConstructUsing(c => new UpdateTemplateCommand(c.OldName, c.Subject, c.Content, c.Name, c.Username));


            _ = this.CreateMap<Email, EmailViewModel>(MemberList.Destination);
            _ = this.CreateMap<Template, TemplateViewModel>(MemberList.Destination);
        }
    }
}
