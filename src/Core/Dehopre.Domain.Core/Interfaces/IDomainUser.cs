namespace Dehopre.Domain.Core.Interfaces
{
    using System;

    /// <summary>
    /// Interface to propagate user id type
    /// </summary>
    public interface IDomainUser
    {
        string UserName { get; }
        string Email { get; }
        bool EmailConfirmed { get; }
        string PasswordHash { get; }
        string SecurityStamp { get; }
        string PhoneNumber { get; }
        bool PhoneNumberConfirmed { get; }
        bool TwoFactorEnabled { get; }
        DateTimeOffset? LockoutEnd { get; }
        bool LockoutEnabled { get; }
        int AccessFailedCount { get; }
        void ConfirmEmail();
    }
}
