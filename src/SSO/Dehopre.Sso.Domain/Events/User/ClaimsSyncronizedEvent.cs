namespace Dehopre.Sso.Domain.Events.User
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Dehopre.Domain.Core.Events;

    public class ClaimsSyncronizedEvent : Event
    {
        public IEnumerable<Claim> Claims { get; }

        public ClaimsSyncronizedEvent(string username, IEnumerable<Claim> claims)
            : base(EventTypes.Success)
        {
            this.Claims = claims;
            this.AggregateId = username;
        }
    }
}
