namespace Dehopre.Sso.Domain.ViewModels.User
{
    using Dehopre.AspNetCore.IQueryable.Extensions.Pagination;
    using Dehopre.AspNetCore.IQueryable.Extensions.Sort;

    public class UserFindByProperties : IQuerySort, IQueryPaging
    {
        public UserFindByProperties(string query) => this.Query = query;

        public string Query { get; }
        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
