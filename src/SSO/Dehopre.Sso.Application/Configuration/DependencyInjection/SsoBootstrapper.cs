namespace Dehopre.Sso.Application.Configuration.DependencyInjection
{
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Configuration;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Sso.Application.CloudServices.Storage;
    using Dehopre.Sso.Application.Interfaces;
    using IdentityServer4.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class SsoBootstrapper
    {
        /// <summary>
        /// Configure SSO Services
        /// </summary>
        /// <typeparam name="THttpUser">Implementation of ISystemUser</typeparam>
        /// <returns></returns>
        public static IDehopreConfigurationBuilder ConfigureSso<THttpUser>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
        {
            services.TryAddScoped<IMediatorHandler, InMemoryBus>();
            services.TryAddScoped<ISystemUser, THttpUser>();
            _ = services.AddScoped<IEventSink, IdentityServerEventStore>();
            _ = services.AddScoped<IStorage, StorageService>();

            _ = services
                .AddApplicationServices()
                .AddDomainEvents()
                .AddDomainCommands();

            return new DehopreBuilder(services);
        }

        /// <summary>   
        /// Configure SSO Services
        /// </summary>
        /// <typeparam name="THttpUser">Implementation of ISystemUser</typeparam>
        /// <returns></returns>
        public static IDehopreConfigurationBuilder ConfigureSso<THttpUser>(this IDehopreConfigurationBuilder builder)
            where THttpUser : class, ISystemUser
        {
            builder.Services.TryAddScoped<IMediatorHandler, InMemoryBus>();
            builder.Services.TryAddScoped<ISystemUser, THttpUser>();
            _ = builder.Services.AddScoped<IEventSink, IdentityServerEventStore>();
            _ = builder.Services.AddScoped<IStorage, StorageService>();
            _ = builder.Services
                .AddApplicationServices()
                .AddDomainEvents()
                .AddDomainCommands();

            return new DehopreBuilder(builder.Services);
        }
    }
}
