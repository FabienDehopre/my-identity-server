namespace Dehopre.Sso.Domain.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Bus;
    using Dehopre.Domain.Core.Commands;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Notifications;
    using Dehopre.Domain.Core.Util;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Events.User;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using Dehopre.Sso.Domain.ViewModels.User;
    using MediatR;

    public class UserCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewUserCommand, bool>,
        IRequestHandler<RegisterNewUserWithoutPassCommand, bool>,
        IRequestHandler<RegisterNewUserWithProviderCommand, bool>,
        IRequestHandler<SendResetLinkCommand, bool>,
        IRequestHandler<ResetPasswordCommand, bool>,
        IRequestHandler<ConfirmEmailCommand, bool>,
        IRequestHandler<AddLoginCommand, bool>
    {
        private readonly IUserService userService;
        private readonly IEmailService emailService;
        private readonly IEmailRepository emailRepository;

        public UserCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IUserService userService,
            IEmailService emailService,
            IEmailRepository emailRepository) : base(uow, bus, notifications)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            this.emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
        }

        public async Task<bool> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var emailAlreadyExists = await this.userService.FindByEmailAsync(request.Email, cancellationToken);
            if (emailAlreadyExists is not null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("New User", "E-mail already exists. If you don't remember your passwork, reset it."), cancellationToken);
                return false;
            }

            var usernameAlreadyExists = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (usernameAlreadyExists is not null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("New User", "Username already exist. If you don't remember your passwork, reset it."), cancellationToken);
                return false;
            }

            var result = await this.userService.CreateUserWithPass(request, request.Password, cancellationToken);
            if (result.HasValue)
            {
                var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
                await this.SendEmailToUser(user, request, result.Value, EmailType.NewUser, cancellationToken);
                await this.Bus.RaiseEvent(new UserRegisteredEvent(result.Value.Username, user.Email), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(RegisterNewUserWithoutPassCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var emailAlreadyExists = await this.userService.FindByEmailAsync(request.Email, cancellationToken);
            if (emailAlreadyExists is not null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("New User", "E-mail already exists. If you don't remember your passwork, reset it."), cancellationToken);
                return false;
            }

            var usernameAlreadyExists = await this.userService.FindByNameAsync(request.Username, cancellationToken);
            if (usernameAlreadyExists is not null)
            {
                await this.Bus.RaiseEvent(new DomainNotification("New User", "Username already exist. If you don't remember your passwork, reset it."), cancellationToken);
                return false;
            }

            var result = await this.userService.CreateUserWithouthPassword(request, cancellationToken);
            if (result.HasValue)
            {
                var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
                await this.SendEmailToUser(user, request, result.Value, EmailType.NewUser, cancellationToken);
                await this.Bus.RaiseEvent(new UserRegisteredEvent(result.Value.Username, user.Email), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(RegisterNewUserWithProviderCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.CreateUserWithProviderAndPass(request, cancellationToken);
            if (result.HasValue)
            {
                var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
                await this.SendEmailToUser(user, request, result.Value, EmailType.NewUser, cancellationToken);
                await this.Bus.RaiseEvent(new UserRegisteredEvent(result.Value.Username, user.Email), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(SendResetLinkCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var accountResult = await this.userService.GenerateResetPasswordLink(request.EmailOrUsername);
            if (accountResult.HasValue)
            {
                var user = await this.userService.FindByNameAsync(request.Username, cancellationToken);
                await this.SendEmailToUser(user, request, accountResult.Value, EmailType.NewUser, cancellationToken);
                await this.Bus.RaiseEvent(new ResetLinkGeneratedEvent(request.Email, request.Username), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var emailSent = await this.userService.ResetPassword(request.Email, request.Code, request.Password, cancellationToken);
            if (emailSent is not null)
            {
                await this.Bus.RaiseEvent(new AccountPasswordResetedEvent(emailSent, request.Email, request.Code), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.ConfirmEmailAsync(request.Email, request.Code, cancellationToken);
            if (result is not null)
            {
                await this.Bus.RaiseEvent(new EmailConfirmedEvent(request.Email, request.Code, result), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<bool> Handle(AddLoginCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await this.NotifyValidationErrors(request, cancellationToken);
                return false;
            }

            var result = await this.userService.AddLoginAsync(request.Email, request.Provider, request.ProviderId, cancellationToken);
            if (result is not null)
            {
                await this.Bus.RaiseEvent(new NewLoginAddedEvent(result, request.Email, request.Provider, request.ProviderId), cancellationToken);
                return true;
            }

            return false;
        }

        private async Task SendEmailToUser(IDomainUser user, UserCommand request, AccountResult accountResult, EmailType type, CancellationToken cancellationToken)
        {
            if (user.EmailConfirmed || !user.Email.IsEmail())
            {
                return;
            }

            var email = await this.emailRepository.GetByType(type, cancellationToken);
            if (email is null)
            {
                return;
            }

            var claims = await this.userService.GetClaimByName(user.UserName, cancellationToken);
            await this.emailService.SendEmailAsync(email.GetMessage(user, accountResult, request, claims), cancellationToken);
        }
    }
}
