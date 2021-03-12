namespace Dehopre.Sso.EntityFramework.Repository.Repository
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.EntityFrameworkCore.Interfaces;
    using Dehopre.EntityFrameworkCore.Repository;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        public EmailRepository(IDehopreEntityFrameworkStore context) : base(context)
        {
        }

        public Task<Email> GetByType(EmailType type, CancellationToken cancellationToken = default) => this.DbSet.FirstOrDefaultAsync(f => f.Type == type, cancellationToken);
    }
}
