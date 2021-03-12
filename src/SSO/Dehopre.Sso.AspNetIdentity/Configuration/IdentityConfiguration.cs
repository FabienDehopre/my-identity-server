namespace Dehopre.Sso.AspNetIdentity.Configuration
{
    using System;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Sso.AspNetIdentity.Models.Identity;
    using Dehopre.Sso.AspNetIdentity.Services;
    using Dehopre.Sso.Domain.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class IdentityConfiguration
    {
        public static IDehopreConfigurationBuilder ConfigureIdentity<TUser, TRole, TKey, TRoleFactory, TUserFactory>(this IDehopreConfigurationBuilder services)
            where TUser : IdentityUser<TKey>, IDomainUser
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
            where TRoleFactory : class, IRoleFactory<TRole>
            where TUserFactory : class, IIdentityFactory<TUser>
        {
            _ = services.Services
                .AddScoped<IUserService, UserService<TUser, TRole, TKey>>()
                .AddScoped<IRoleService, RoleService<TRole, TKey>>()
                .AddScoped<IIdentityFactory<TUser>, TUserFactory>()
                .AddScoped<IRoleFactory<TRole>, TRoleFactory>();

            return services;
        }

        public static IDehopreConfigurationBuilder AddDefaultAspNetIdentityServices(this IDehopreConfigurationBuilder services)
        {
            // Infra - Identity Services
            _ = services.Services.AddScoped<IUserService, UserService<UserIdentity, RoleIdentity, string>>()
                .AddScoped<IRoleService, RoleService<RoleIdentity, string>>()
                .AddScoped<IIdentityFactory<UserIdentity>, IdentityFactory>()
                .AddScoped<IRoleFactory<RoleIdentity>, IdentityFactory>();

            return services;
        }
    }
}
