namespace Dehopre.Sso.EntityFramework.Repository.Configuration
{
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.EntityFrameworkCore.Repository;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.EntityFramework.Repository.Repository;
    using Microsoft.Extensions.DependencyInjection;

    internal static class RepositoryBootStrapper
    {
        public static IServiceCollection AddStores(this IServiceCollection services)
        {
            // Infra - Data EventSourcing
            _ = services.AddScoped<IEventStoreRepository, EventStoreRepository>()
                .AddScoped<ITemplateRepository, TemplateRepository>()
                .AddScoped<IGlobalConfigurationSettingsRepository, GlobalConfigurationSettingsRepository>()
                .AddScoped<IEmailRepository, EmailRepository>();
            return services;
        }
    }
}
