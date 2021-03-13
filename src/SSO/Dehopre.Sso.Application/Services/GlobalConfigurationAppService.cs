namespace Dehopre.Sso.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Sso.Application.AutoMapper;
    using Dehopre.Sso.Application.Interfaces;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Domain.Commands.GlobalConfiguration;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.ViewModels.Settings;
    using global::AutoMapper;

    public class GlobalConfigurationAppService : IGlobalConfigurationAppService
    {
        public IMediatorHandler Bus { get; }
        private readonly IMapper mapper;
        private readonly IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository;

        private readonly ISystemUser systemUser;

        public GlobalConfigurationAppService(
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository,
            ISystemUser systemUser,
            IMediatorHandler bus)
        {
            this.Bus = bus;
            this.mapper = GlobalConfigurationMapping.Mapper;
            this.globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;

            this.systemUser = systemUser;
        }

        public async Task<PrivateSettings> GetPrivateSettings(CancellationToken cancellationToken = default)
        {
            var settings = await this.globalConfigurationSettingsRepository.All(cancellationToken);
            var privateSettings = new PrivateSettings(new Settings(settings));

            return privateSettings;
        }

        public async Task<PublicSettings> GetPublicSettings(CancellationToken cancellationToken = default)
        {
            var settings = await this.globalConfigurationSettingsRepository.All(cancellationToken);

            var publicSettings = new PublicSettings(new Settings(settings));

            return publicSettings;
        }

        public async Task<bool> UpdateSettings(IEnumerable<ConfigurationViewModel> configs, CancellationToken cancellationToken = default)
        {
            var success = true;
            foreach (var configurationViewModel in configs)
            {
                success = await this.Bus.SendCommand(this.mapper.Map<ManageConfigurationCommand>(configurationViewModel), cancellationToken);
                if (!success)
                {
                    break;
                }
            }
            return success;
        }

        public async Task<IEnumerable<ConfigurationViewModel>> ListSettings(CancellationToken cancellationToken = default)
        {
            var settings = this.mapper.Map<IEnumerable<ConfigurationViewModel>>(await this.globalConfigurationSettingsRepository.All(cancellationToken));
            if (!this.systemUser.IsInRole("Administrator"))
            {
                foreach (var configurationViewModel in settings)
                {
                    configurationViewModel.UpdateSensitive();
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            return settings;
        }
    }
}
