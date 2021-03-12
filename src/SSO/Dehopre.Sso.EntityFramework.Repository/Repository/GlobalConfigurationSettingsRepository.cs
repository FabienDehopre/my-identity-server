namespace Dehopre.Sso.EntityFramework.Repository.Repository
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.EntityFrameworkCore.Interfaces;
    using Dehopre.EntityFrameworkCore.Repository;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class GlobalConfigurationSettingsRepository : Repository<GlobalConfigurationSettings>, IGlobalConfigurationSettingsRepository
    {
        public GlobalConfigurationSettingsRepository(IDehopreEntityFrameworkStore context) : base(context)
        {
        }

        public Task<List<GlobalConfigurationSettings>> All(CancellationToken cancellationToken = default) => this.DbSet.ToListAsync(cancellationToken);

        public Task<GlobalConfigurationSettings> FindByKey(string key, CancellationToken cancellationToken = default) => this.DbSet.FirstOrDefaultAsync(f => f.Key == key, cancellationToken);
    }
}
