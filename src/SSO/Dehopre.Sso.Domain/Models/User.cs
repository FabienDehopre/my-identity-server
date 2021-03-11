namespace Dehopre.Sso.Domain.Models
{
    using System;
    using Dehopre.Domain.Core.Interfaces;

    public class User : IDomainUser
    {
        public User() { }

        public string Id { get; private set; }
        public string Email { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public string PasswordHash { get; private set; }
        public string SecurityStamp { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool PhoneNumberConfirmed { get; private set; }
        public bool TwoFactorEnabled { get; private set; }
        public DateTimeOffset? LockoutEnd { get; private set; }
        public bool LockoutEnabled { get; private set; }
        public int AccessFailedCount { get; private set; }
        public string UserName { get; private set; }

        public void ConfirmEmail() => throw new NotImplementedException();
    }
}
