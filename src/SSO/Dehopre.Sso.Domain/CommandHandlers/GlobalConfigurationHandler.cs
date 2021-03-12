namespace Dehopre.Sso.Domain.CommandHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Commands;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Notifications;
    using Dehopre.Sso.Domain.Commands.GlobalConfiguration;
    using Dehopre.Sso.Domain.Events.GlobalConfiguration;
    using Dehopre.Sso.Domain.Interfaces;
    using MediatR;

    public class GlobalConfigurationHandler : CommandHandler, IRequestHandler<ManageConfigurationCommand, bool>
    {
        private readonly IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository;

        public GlobalConfigurationHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository)
            : base(uow, bus, notifications)
            => this.globalConfigurationSettingsRepository = globalConfigurationSettingsRepository ?? throw new ArgumentNullException(nameof(globalConfigurationSettingsRepository));

        public async Task<bool> Handle(ManageConfigurationCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var settings = await this.globalConfigurationSettingsRepository.FindByKey(request.Key, cancellationToken);
            if (settings is null)
            {
                return await this.CreateConfiguration(request, cancellationToken);
            }

            return await this.UpdateConfiguration(settings, request, cancellationToken);
        }

        private async Task<bool> CreateConfiguration(ManageConfigurationCommand request, CancellationToken cancellationToken)
        {
            this.globalConfigurationSettingsRepository.Add(request.ToEntity());

            if (await this.Commit(cancellationToken))
            {
                await this.Bus.RaiseEvent(new GlobalConfigurationCreatedEvent(request.Key, request.Sensitive ? "Sentitive information" : request.Value, request.IsPublic, request.Sensitive), cancellationToken);
                return true;
            }

            return false;
        }

        private async Task<bool> UpdateConfiguration(Models.GlobalConfigurationSettings settings, ManageConfigurationCommand request, CancellationToken cancellationToken)
        {
            settings.Update(request.Value, request.IsPublic, request.Sensitive);
            this.globalConfigurationSettingsRepository.Update(settings);

            if (await this.Commit(cancellationToken))
            {
                await this.Bus.RaiseEvent(new GlobalConfigurationUpdatedEvent(request.Key, request.Sensitive ? "Sentitive information" : request.Value, request.IsPublic, request.Sensitive), cancellationToken);
                return true;
            }

            return false;
        }
    }
}
