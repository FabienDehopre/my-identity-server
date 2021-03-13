namespace Dehopre.Sso.Application.Configuration
{
    using Dehopre.Sso.Application.CloudServices.Email;
    using Dehopre.Sso.Application.Interfaces;
    using Dehopre.Sso.Application.Services;
    using Dehopre.Sso.Domain.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    internal static class ApplicationBootStrapper
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            _ = services.AddScoped<IUserAppService, UserAppService>();
            _ = services.AddScoped<IUserManageAppService, UserManagerAppService>();
            _ = services.AddScoped<IRoleManagerAppService, RoleManagerAppService>();
            _ = services.AddScoped<IEmailAppService, EmailAppService>();
            _ = services.AddScoped<IEventStoreAppService, EventStoreAppService>();

            _ = services.AddTransient<IEmailService, GeneralSmtpService>();
            _ = services.AddTransient<IGlobalConfigurationSettingsService, GlobalConfigurationAppService>();
            _ = services.AddTransient<IGlobalConfigurationAppService, GlobalConfigurationAppService>();

            return services;
        }
    }
}
