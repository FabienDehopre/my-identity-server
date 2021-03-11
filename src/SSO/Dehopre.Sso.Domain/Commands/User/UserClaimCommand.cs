namespace Dehopre.Sso.Domain.Commands.User
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Dehopre.Domain.Core.Commands;

    public abstract class UserClaimCommand : Command
    {
        public int Id { get; protected set; }
        public string Username { get; protected set; }
        public string Type { get; protected set; }

        public string Value { get; protected set; }
        public IEnumerable<Claim> Claims { get; protected set; }

    }
}
