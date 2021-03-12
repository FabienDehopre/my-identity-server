namespace Dehopre.Sso.AspNetIdentity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Notifications;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class RoleService<TRole, TKey> : IRoleService
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly RoleManager<TRole> roleManager;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<IdentityRole<TKey>> logger;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly IMediatorHandler bus;
        private readonly IRoleFactory<TRole> roleFactory;

        public RoleService(
            RoleManager<TRole> roleManager,
            IMediatorHandler bus,
            IRoleFactory<TRole> roleFactory,
            ILoggerFactory loggerFactory)
        {
            this.roleManager = roleManager;
            this.bus = bus;
            this.roleFactory = roleFactory;
            this.logger = loggerFactory.CreateLogger<IdentityRole<TKey>>();
        }

        public async Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken = default)
        {
            var roles = await this.roleManager.Roles.ToListAsync(cancellationToken);
            return roles.Select(s => new Role(s.Id.ToString(), s.Name)).ToList();
        }

        public async Task<bool> Remove(string name, CancellationToken cancellationToken = default)
        {
            var roleClaim = await this.roleManager.Roles.Where(x => x.Name == name).SingleOrDefaultAsync(cancellationToken);
            var result = await this.roleManager.DeleteAsync(roleClaim);
            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }

        public async Task<Role> Details(string name, CancellationToken cancellationToken = default)
        {
            var s = await this.roleManager.Roles.FirstAsync(f => f.Name == name, cancellationToken);
            return new Role(s.Id.ToString(), s.Name);
        }

        public async Task<bool> Save(string name, CancellationToken cancellationToken = default)
        {
            var role = this.roleFactory.CreateRole(name);
            var result = await this.roleManager.CreateAsync(role);
            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> Update(string name, string oldName, CancellationToken cancellationToken = default)
        {
            var s = await this.roleManager.Roles.FirstAsync(f => f.Name == oldName, cancellationToken);
            s.Name = name;
            var result = await this.roleManager.UpdateAsync(s);
            foreach (var error in result.Errors)
            {
                await this.bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return result.Succeeded;
        }
    }
}
