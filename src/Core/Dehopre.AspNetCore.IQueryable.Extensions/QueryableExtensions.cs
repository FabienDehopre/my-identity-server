namespace Dehopre.AspNetCore.IQueryable.Extensions
{
    using System.Linq;
    using Dehopre.AspNetCore.IQueryable.Extensions.Filter;
    using Dehopre.AspNetCore.IQueryable.Extensions.Pagination;
    using Dehopre.AspNetCore.IQueryable.Extensions.Sort;

    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Apply<TEntity>(this IQueryable<TEntity> result, ICustomQueryable model)
        {
            result = result.Filter(model);
            if (model is IQuerySort sort)
            {
                result = result.Sort(sort);
            }

            if (model is IQueryPaging pagination)
            {
                result = result.Paginate(pagination);
            }

            return result;
        }
    }
}
