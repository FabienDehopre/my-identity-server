namespace Dehopre.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Util;
    using IdentityModel;
    using Microsoft.AspNetCore.Http;

    public class AspNetUser : ISystemUser
    {
        private readonly IHttpContextAccessor accessor;

        public AspNetUser(IHttpContextAccessor accessor) => this.accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));

        public string Username => this.GetUsername();

        public string UserId => this.accessor.HttpContext.User.FindFirst(JwtClaimTypes.Subject)?.Value;

        public bool IsAuthenticated() => this.accessor.HttpContext.User.Identity.IsAuthenticated;

        public bool IsInRole(string role) => this.accessor.HttpContext.User.IsInRole(role);

        public IEnumerable<Claim> GetClaimsIdentity() => this.accessor.HttpContext.User.Claims;

        public string GetRemoteIpAddress() => this.accessor.HttpContext.Connection.RemoteIpAddress.ToString();

        public string GetLocalIpAddress() => this.accessor.HttpContext.Connection.LocalIpAddress.ToString();

        private string GetUsername()
        {
            var username = this.accessor.HttpContext.User.FindFirst("username")?.Value;
            if (username.IsPresent())
            {
                return username;
            }

            username = this.accessor.HttpContext.User.FindFirst(JwtClaimTypes.Name)?.Value;
            if (username.IsPresent())
            {
                return username;
            }

            username = this.accessor.HttpContext.User.FindFirst(JwtClaimTypes.GivenName)?.Value;
            if (username.IsPresent())
            {
                return username;
            }

            username = this.accessor.HttpContext.User.Identity.Name;
            if (username.IsPresent())
            {
                return username;
            }

            var sub = this.accessor.HttpContext.User.FindFirst(JwtClaimTypes.Subject);
            if (sub != null)
            {
                return sub.Value;
            }

            return string.Empty;
        }
    }
}
