namespace Dehopre.Sso.Application.Configuration
{
    using Dehopre.Sso.Domain.CommandHandlers;
    using Dehopre.Sso.Domain.Commands.Email;
    using Dehopre.Sso.Domain.Commands.GlobalConfiguration;
    using Dehopre.Sso.Domain.Commands.Role;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    internal static class DomainCommandsBootStrapper
    {
        public static IServiceCollection AddDomainCommands(this IServiceCollection services)
        {
            /*
             * Role commands
             */
            _ = services.AddScoped<IRequestHandler<RemoveRoleCommand, bool>, RoleCommandHandler>();
            _ = services.AddScoped<IRequestHandler<RemoveUserFromRoleCommand, bool>, RoleCommandHandler>();
            _ = services.AddScoped<IRequestHandler<UpdateRoleCommand, bool>, RoleCommandHandler>();
            _ = services.AddScoped<IRequestHandler<SaveRoleCommand, bool>, RoleCommandHandler>();

            /*
             * Regiser commands
             */
            _ = services.AddScoped<IRequestHandler<RegisterNewUserCommand, bool>, UserCommandHandler>();
            _ = services.AddScoped<IRequestHandler<RegisterNewUserWithoutPassCommand, bool>, UserCommandHandler>();
            _ = services.AddScoped<IRequestHandler<RegisterNewUserWithProviderCommand, bool>, UserCommandHandler>();
            _ = services.AddScoped<IRequestHandler<SendResetLinkCommand, bool>, UserCommandHandler>();
            _ = services.AddScoped<IRequestHandler<ResetPasswordCommand, bool>, UserCommandHandler>();
            _ = services.AddScoped<IRequestHandler<ConfirmEmailCommand, bool>, UserCommandHandler>();
            _ = services.AddScoped<IRequestHandler<AddLoginCommand, bool>, UserCommandHandler>();


            /*
             * User manager
             */
            _ = services.AddScoped<IRequestHandler<UpdateProfileCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<UpdateProfilePictureCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<SetPasswordCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<ChangePasswordCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<RemoveAccountCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<AdminUpdateUserCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<SaveUserClaimCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<RemoveUserClaimCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<SaveUserRoleCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<RemoveUserRoleCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<AdminChangePasswordCommand, bool>, UserManagementCommandHandler>();
            _ = services.AddScoped<IRequestHandler<SynchronizeClaimsCommand, bool>, UserManagementCommandHandler>();


            /*
             * Template
             */
            _ = services.AddScoped<IRequestHandler<SaveTemplateCommand, bool>, EmailCommandHandler>();
            _ = services.AddScoped<IRequestHandler<UpdateTemplateCommand, bool>, EmailCommandHandler>();
            _ = services.AddScoped<IRequestHandler<SaveEmailCommand, bool>, EmailCommandHandler>();
            _ = services.AddScoped<IRequestHandler<RemoveTemplateCommand, bool>, EmailCommandHandler>();

            /*
            * Global Config
            */
            _ = services.AddScoped<IRequestHandler<ManageConfigurationCommand, bool>, GlobalConfigurationHandler>();

            return services;
        }
    }
}
