namespace Dehopre.Sso.Application.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Application.ViewModels.UserViewModels;

    public interface IUserAppService : IDisposable
    {
        /// <summary>
        /// Register user as an admin. Bypass many validation rules
        /// </summary>
        Task<bool> AdminRegister(AdminRegisterUserViewModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Register regular user. With password and without Provider
        /// </summary>
        Task<bool> Register(RegisterUserViewModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Register user from LDAP connection
        /// </summary>
        Task<bool> Register(RegisterUserLdapViewModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Register user and add a new Login for him. Usually for federation logins
        /// </summary>
        Task<bool> RegisterWithoutPassword(SocialViewModel model, CancellationToken cancellationToken = default);

        Task<bool> CheckUsername(string userName, CancellationToken cancellationToken = default);
        Task<bool> CheckEmail(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Register user with password and add a new Login for him.
        /// </summary>
        Task<bool> RegisterWithPasswordAndProvider(RegisterUserViewModel model, CancellationToken cancellationToken = default);
        Task<bool> SendResetLink(ForgotPasswordViewModel model, CancellationToken cancellationToken = default);
        Task<bool> ResetPassword(ResetPasswordViewModel model, CancellationToken cancellationToken = default);
        Task<bool> ConfirmEmail(ConfirmEmailViewModel model, CancellationToken cancellationToken = default);
        Task<bool> AddLogin(SocialViewModel user, CancellationToken cancellationToken = default);
    }
}
