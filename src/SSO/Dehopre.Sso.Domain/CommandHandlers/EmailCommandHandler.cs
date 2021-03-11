namespace Dehopre.Sso.Domain.CommandHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Commands;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Notifications;
    using Dehopre.Sso.Domain.Commands.Email;
    using Dehopre.Sso.Domain.Events.Email;
    using Dehopre.Sso.Domain.Interfaces;
    using MediatR;

    public class EmailCommandHandler : CommandHandler, IRequestHandler<SaveTemplateCommand, bool>, IRequestHandler<UpdateTemplateCommand, bool>, IRequestHandler<SaveEmailCommand, bool>, IRequestHandler<RemoveTemplateCommand, bool>
    {
        private readonly ITemplateRepository templateRepository;
        private readonly IEmailRepository emailRepository;

        public EmailCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            ITemplateRepository templateRepository,
            IEmailRepository emailRepository) : base(uow, bus, notifications)
        {
            this.templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
            this.emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
        }

        public async Task<bool> Handle(SaveTemplateCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                this.NotifyValidationErrors(request);
                return false;
            }

            var template = request.ToModel();
            var templateAlreadyExists = await this.templateRepository.Exist(template.Name);
            if (templateAlreadyExists)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Template", "Template already exists."), cancellationToken);
                return false;
            }

            this.templateRepository.Add(template);
            if (await this.Commit(cancellationToken))
            {
                await this.Bus.RaiseEvent(new TemplateAddedEvent(template), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                this.NotifyValidationErrors(request);
                return false;
            }

            var template = await this.templateRepository.GetByName(request.OldName);
            if (template == null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Template", "Template not found."), cancellationToken);
                return false;
            }

            template.UpdateTemplate(request.Content, request.Subject, request.Name, request.UserName);
            this.templateRepository.Update(template);

            if (await this.Commit(cancellationToken))
            {
                await this.Bus.RaiseEvent(new TemplateAddedEvent(template), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(SaveEmailCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                this.NotifyValidationErrors(request);
                return false;
            }

            var email = await this.emailRepository.GetByType(request.Type);
            if (email == null)
            {
                email = request.ToModel();
                this.emailRepository.Add(email);
            }
            else
            {
                email.Update(request);
                this.emailRepository.Update(email);
            }

            if (await this.Commit(cancellationToken))
            {
                await this.Bus.RaiseEvent(new EmailSavedEvent(email), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(RemoveTemplateCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                this.NotifyValidationErrors(request);
                return false;
            }

            var template = await this.templateRepository.GetByName(request.Name);
            if (template == null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("Template", "Template not found."), cancellationToken);
                return false;
            }

            this.templateRepository.Remove(template);

            if (await this.Commit(cancellationToken))
            {
                await this.Bus.RaiseEvent(new TemplateRemovedEvent(template.Id), cancellationToken);
                return true;
            }

            return false;
        }
    }
}
