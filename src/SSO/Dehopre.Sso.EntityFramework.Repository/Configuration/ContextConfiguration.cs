namespace Dehopre.Sso.EntityFramework.Repository.Configuration
{
    using Dehopre.Domain.Core.Events;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.EntityFrameworkCore.EventSourcing;
    using Dehopre.EntityFrameworkCore.Interfaces;
    using Dehopre.EntityFrameworkCore.Repository;
    using Dehopre.Sso.EntityFramework.Repository.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ContextConfiguration
    {
        public static IDehopreConfigurationBuilder AddSsoContext<TContext, TEventStore>(this IDehopreConfigurationBuilder services)
            where TContext : class, ISsoContext
            where TEventStore : class, IEventStoreContext
        {
            services.Services.AddScoped<ISsoContext, TContext>()
                .AddStores()
                .TryAddScoped<IEventStore, SqlEventStore>();
            services.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.Services.TryAddScoped<IEventStoreContext, TEventStore>();
            services.Services.TryAddScoped<IDehopreEntityFrameworkStore>(x => x.GetRequiredService<TContext>());
            return services;
        }

        public static IDehopreConfigurationBuilder AddSsoContext<TContext>(this IDehopreConfigurationBuilder services)
            where TContext : class, ISsoContext, IEventStoreContext

        {
            services.Services.AddScoped<ISsoContext, TContext>()
                .AddStores()
                .TryAddScoped<IEventStore, SqlEventStore>();
            services.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.Services.TryAddScoped<IEventStoreContext>(s => s.GetService<TContext>());
            services.Services.TryAddScoped<IDehopreEntityFrameworkStore>(x => x.GetRequiredService<TContext>());
            return services;
        }
    }
}
