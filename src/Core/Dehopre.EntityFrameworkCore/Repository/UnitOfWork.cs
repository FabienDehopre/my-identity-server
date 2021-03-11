namespace Dehopre.EntityFrameworkCore.Repository
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.EntityFrameworkCore.Interfaces;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDehopreEntityFrameworkStore context;
        private readonly IEventStoreContext eventStoreContext;

        public UnitOfWork(IDehopreEntityFrameworkStore context, IEventStoreContext eventStoreContext)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.eventStoreContext = eventStoreContext ?? throw new ArgumentNullException(nameof(eventStoreContext));
        }

        public async Task<bool> Commit(CancellationToken cancellationToken = default)
        {
            var linesModified = await this.context.SaveChangesAsync(cancellationToken);
            if (this.eventStoreContext.GetType() != this.context.GetType())
            {
                _ = await this.eventStoreContext.SaveChangesAsync(cancellationToken);
            }

            return linesModified > 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.context.Dispose();
            this.eventStoreContext.Dispose();
        }
    }
}
