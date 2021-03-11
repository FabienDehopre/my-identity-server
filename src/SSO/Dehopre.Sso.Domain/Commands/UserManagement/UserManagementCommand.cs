namespace Dehopre.Sso.Domain.Commands.UserManagement
{
    using System;
    using Dehopre.Domain.Core.Commands;

    public abstract class UserManagementCommand : Command
    {
        public string Email { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string Name { get; protected set; }
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string ConfirmPassword { get; protected set; }
        public string Provider { get; protected set; }
        public string ProviderId { get; protected set; }
        public bool EmailConfirmed { get; protected set; }
        public bool PhoneNumberConfirmed { get; protected set; }
        public bool TwoFactorEnabled { get; protected set; }
        public DateTimeOffset? LockoutEnd { get; protected set; }
        public bool LockoutEnabled { get; protected set; }
        public int AccessFailedCount { get; protected set; }
        public string SocialNumber { get; internal set; }
        public DateTime? Birthdate { get; internal set; }
    }
}
