namespace Dehopre.Domain.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public interface ISystemUser
    {
        string Username { get; }
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
        string GetRemoteIpAddress();
        string GetLocalIpAddress();
    }
}
