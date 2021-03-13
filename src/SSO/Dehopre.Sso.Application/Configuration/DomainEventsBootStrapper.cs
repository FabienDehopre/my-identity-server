namespace Dehopre.Sso.Application.Configuration
{
    using Dehopre.Domain.Core.Notifications;
    using Dehopre.Sso.Domain.EventHandlers;
    using Dehopre.Sso.Domain.Events.User;
    using Dehopre.Sso.Domain.Events.UserManagement;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    internal static class DomainEventsBootStrapper
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.TryAddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            _ = services.AddScoped<INotificationHandler<UserRegisteredEvent>, UserEventHandler>();
            _ = services.AddScoped<INotificationHandler<EmailConfirmedEvent>, UserEventHandler>();
            _ = services.AddScoped<INotificationHandler<ProfileUpdatedEvent>, UserManagerEventHandler>();
            _ = services.AddScoped<INotificationHandler<ProfilePictureUpdatedEvent>, UserManagerEventHandler>();
            _ = services.AddScoped<INotificationHandler<PasswordCreatedEvent>, UserManagerEventHandler>();
            _ = services.AddScoped<INotificationHandler<PasswordChangedEvent>, UserManagerEventHandler>();
            _ = services.AddScoped<INotificationHandler<AccountRemovedEvent>, UserManagerEventHandler>();

            return services;
        }
    }
}
