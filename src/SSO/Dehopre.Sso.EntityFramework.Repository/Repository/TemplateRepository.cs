namespace Dehopre.Sso.EntityFramework.Repository.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.EntityFrameworkCore.Interfaces;
    using Dehopre.EntityFrameworkCore.Repository;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(IDehopreEntityFrameworkStore context) : base(context)
        {
        }

        public Task<List<Template>> All(CancellationToken cancellationToken = default) => this.DbSet.ToListAsync(cancellationToken);
        public Task<bool> Exist(string name, CancellationToken cancellationToken = default) => this.DbSet.AnyAsync(w => w.Name.ToUpper() == name.ToUpper(), cancellationToken);
        public Task<Template> GetByName(string name, CancellationToken cancellationToken = default) => this.DbSet.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
    }
}
