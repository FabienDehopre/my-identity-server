namespace Dehopre.Sso.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Sso.Application.AutoMapper;
    using Dehopre.Sso.Application.Interfaces;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Application.ViewModels.UserViewModels;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Interfaces;
    using global::AutoMapper;

    public class UserAppService : IUserAppService
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IMediatorHandler bus;

        public UserAppService(
            IUserService userService,
            IMediatorHandler bus)
        {

            this.mapper = UserMapping.Mapper;
            this.userService = userService;
            this.bus = bus;
        }

        /// <summary>
        /// Register user as an admin. Bypass many validation rules
        /// </summary>
        public Task<bool> AdminRegister(AdminRegisterUserViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RegisterNewUserCommand>(model), cancellationToken);

        /// <summary>
        /// Register regular user. With password and without Provider
        /// </summary>
        public Task<bool> Register(RegisterUserViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RegisterNewUserCommand>(model), cancellationToken);

        /// <summary>
        /// Register user from LDAP connection
        /// </summary>
        public Task<bool> Register(RegisterUserLdapViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RegisterNewUserWithoutPassCommand>(model), cancellationToken);

        /// <summary>
        /// Register user and add a new Login for him. Usually for federation logins
        /// </summary>
        public Task<bool> RegisterWithoutPassword(SocialViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RegisterNewUserWithoutPassCommand>(model), cancellationToken);

        /// <summary>
        /// Register user with password and add a new Login for him.
        /// </summary>
        public Task<bool> RegisterWithPasswordAndProvider(RegisterUserViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<RegisterNewUserWithProviderCommand>(model), cancellationToken);

        public Task<bool> SendResetLink(ForgotPasswordViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<SendResetLinkCommand>(model), cancellationToken);

        public Task<bool> ResetPassword(ResetPasswordViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<ResetPasswordCommand>(model), cancellationToken);

        public Task<bool> ConfirmEmail(ConfirmEmailViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<ConfirmEmailCommand>(model), cancellationToken);


        public Task<bool> AddLogin(SocialViewModel model, CancellationToken cancellationToken = default) => this.bus.SendCommand(this.mapper.Map<AddLoginCommand>(model), cancellationToken);

        public Task<bool> CheckUsername(string userName, CancellationToken cancellationToken = default) => this.userService.UsernameExist(userName, cancellationToken);

        public Task<bool> CheckEmail(string email, CancellationToken cancellationToken = default) => this.userService.EmailExist(email, cancellationToken);

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
