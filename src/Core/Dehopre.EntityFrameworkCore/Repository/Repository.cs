namespace Dehopre.EntityFrameworkCore.Repository
{
    using System;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.EntityFrameworkCore.Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Generic repository for <see cref="T:JPProject.EntityFrameworkCore.Interfaces.IDehopreEntityFrameworkStore" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
#pragma warning disable IDE1006 // Naming Styles
        protected readonly IDehopreEntityFrameworkStore Db;
        protected readonly DbSet<TEntity> DbSet;
#pragma warning restore IDE1006 // Naming Styles

        public Repository(IDehopreEntityFrameworkStore context)
        {
            this.Db = context;
            this.DbSet = this.Db.Set<TEntity>();
        }

        public virtual void Add(TEntity obj) => this.DbSet.Add(obj);

        public virtual void Update(TEntity obj) => this.DbSet.Update(obj);

        public virtual void Remove<T>(T id) => this.DbSet.Remove(this.DbSet.Find(id));

        public void Remove(TEntity obj) => this.DbSet.Remove(obj);

        public void Dispose()
        {
            this.Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
