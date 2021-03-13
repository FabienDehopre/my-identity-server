namespace Dehopre.Sso.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Sso.Application.AutoMapper;
    using Dehopre.Sso.Application.Interfaces;
    using Dehopre.Sso.Application.ViewModels.EmailViewModels;
    using Dehopre.Sso.Domain.Commands.Email;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using global::AutoMapper;

    public class EmailAppService : IEmailAppService
    {
        private readonly IMapper mapper;
        private readonly IEmailRepository emailRepository;
        private readonly ITemplateRepository templateRepository;

        public IMediatorHandler Bus { get; set; }

        public EmailAppService(
            IMediatorHandler bus,
            IEmailRepository emailRepository,
            ITemplateRepository templateRepository)
        {
            this.mapper = EmailMapping.Mapper;
            this.emailRepository = emailRepository;
            this.templateRepository = templateRepository;
            this.Bus = bus;
        }

        public async Task<EmailViewModel> FindByType(EmailType type, CancellationToken cancellationToken = default) => this.mapper.Map<EmailViewModel>(await this.emailRepository.GetByType(type, cancellationToken));

        public Task<bool> SaveEmail(EmailViewModel model, CancellationToken cancellationToken = default) => this.Bus.SendCommand(this.mapper.Map<SaveEmailCommand>(model), cancellationToken);

        public async Task<IEnumerable<TemplateViewModel>> ListTemplates(CancellationToken cancellationToken = default) => this.mapper.Map<IEnumerable<TemplateViewModel>>(await this.templateRepository.All(cancellationToken));

        public async Task<TemplateViewModel> GetTemplate(string name, CancellationToken cancellationToken = default) => this.mapper.Map<TemplateViewModel>(await this.templateRepository.GetByName(name, cancellationToken));

        public Task<bool> RemoveTemplate(string name, CancellationToken cancellationToken = default) => this.Bus.SendCommand(new RemoveTemplateCommand(name), cancellationToken);

        public Task<bool> SaveTemplate(TemplateViewModel model, CancellationToken cancellationToken = default) => this.Bus.SendCommand(this.mapper.Map<SaveTemplateCommand>(model), cancellationToken);

        public Task<bool> UpdateTemplate(TemplateViewModel model, CancellationToken cancellationToken = default) => this.Bus.SendCommand(this.mapper.Map<UpdateTemplateCommand>(model), cancellationToken);

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
