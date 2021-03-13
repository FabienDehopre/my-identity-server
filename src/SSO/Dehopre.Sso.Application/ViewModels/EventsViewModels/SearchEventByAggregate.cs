namespace Dehopre.Sso.Application.ViewModels.EventsViewModels
{
    using Dehopre.AspNetCore.IQueryable.Extensions.Attributes;
    using Dehopre.AspNetCore.IQueryable.Extensions.Filter;
    using Dehopre.AspNetCore.IQueryable.Extensions.Pagination;
    using Dehopre.AspNetCore.IQueryable.Extensions.Sort;
    using Dehopre.Domain.Core.ViewModels;

    public class SearchEventByAggregate : ICustomEventQueryable, IQuerySort, IQueryPaging
    {
        [QueryOperator(HasName = "AggregateId", Operator = WhereOperator.Equals)]
        public string Aggregate { get; set; }

        public string Sort { get; set; }

        [QueryOperator(Max = 100)]
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
