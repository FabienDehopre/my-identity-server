namespace Dehopre.Sso.Domain.Interfaces
{
    using Dehopre.AspNetCore.IQueryable.Extensions;
    using Dehopre.AspNetCore.IQueryable.Extensions.Pagination;
    using Dehopre.AspNetCore.IQueryable.Extensions.Sort;

    public interface IUserSearch : ICustomQueryable, IQuerySort, IQueryPaging
    {
    }
}
