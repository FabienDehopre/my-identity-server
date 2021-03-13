namespace Dehopre.Sso.Application.AutoMapper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Events;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.ViewModels;
    using Dehopre.Sso.Application.EventSourcedNormalizers;
    using Dehopre.Sso.Application.ViewModels;
    using Dehopre.Sso.Application.ViewModels.UserViewModels;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using Dehopre.Sso.Domain.Models;
    using global::AutoMapper;

    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            /*
          * User Creation Commands
          */
            _ = this.CreateMap<RegisterUserViewModel, RegisterNewUserCommand>().ConstructUsing(c => new RegisterNewUserCommand(c.Username, c.Email, c.Name, c.PhoneNumber, c.Password, c.ConfirmPassword, c.Birthdate, c.SocialNumber, true));
            _ = this.CreateMap<AdminRegisterUserViewModel, RegisterNewUserCommand>().ConstructUsing(c => new RegisterNewUserCommand(c.Username, c.Email, c.Name, c.PhoneNumber, c.Password, c.ConfirmPassword, c.Birthdate, c.SocialNumber, c.ConfirmEmail));
            _ = this.CreateMap<SocialViewModel, RegisterNewUserWithoutPassCommand>(MemberList.Source).ConstructUsing(c => new RegisterNewUserWithoutPassCommand(c.Email, c.Email, c.Name, c.Picture, c.Provider, c.ProviderId, true));
            _ = this.CreateMap<RegisterUserLdapViewModel, RegisterNewUserWithoutPassCommand>(MemberList.Source).ConstructUsing(c => new RegisterNewUserWithoutPassCommand(c.Email, c.Email, c.Name, c.Picture, null, null, false));

            _ = this.CreateMap<RegisterUserViewModel, RegisterNewUserWithProviderCommand>().ConstructUsing(c => new RegisterNewUserWithProviderCommand(c.Username, c.Email, c.Name, c.PhoneNumber, c.Password, c.ConfirmPassword, c.Picture, c.Provider, c.ProviderId, c.Birthdate, c.SocialNumber));
            _ = this.CreateMap<ForgotPasswordViewModel, SendResetLinkCommand>().ConstructUsing(c => new SendResetLinkCommand(c.UsernameOrEmail));
            _ = this.CreateMap<ResetPasswordViewModel, ResetPasswordCommand>().ConstructUsing(c => new ResetPasswordCommand(c.Password, c.ConfirmPassword, c.Code, c.Email));
            _ = this.CreateMap<ConfirmEmailViewModel, ConfirmEmailCommand>().ConstructUsing(c => new ConfirmEmailCommand(c.Code, c.Email));
            _ = this.CreateMap<SocialViewModel, AddLoginCommand>().ConstructUsing(c => new AddLoginCommand(c.Email, c.Provider, c.ProviderId));
            /*
             * User Management commands
             */
            _ = this.CreateMap<UserViewModel, UpdateProfileCommand>().ConstructUsing(c => new UpdateProfileCommand(c.UserName, c.Url, c.Bio, c.Company, c.JobTitle, c.Name, c.PhoneNumber, c.SocialNumber, c.Birthdate));
            _ = this.CreateMap<UserViewModel, AdminUpdateUserCommand>().ConstructUsing(c => new AdminUpdateUserCommand(c.Email, c.UserName, c.Name, c.PhoneNumber, c.EmailConfirmed, c.PhoneNumberConfirmed, c.TwoFactorEnabled, c.LockoutEnd, c.LockoutEnabled, c.AccessFailedCount, c.Birthdate, c.SocialNumber));
            _ = this.CreateMap<ProfilePictureViewModel, UpdateProfilePictureCommand>().ConstructUsing(c => new UpdateProfilePictureCommand(c.Username, c.Picture));
            _ = this.CreateMap<ChangePasswordViewModel, ChangePasswordCommand>().ConstructUsing(c => new ChangePasswordCommand(c.Username, c.OldPassword, c.NewPassword, c.ConfirmPassword));
            _ = this.CreateMap<SetPasswordViewModel, SetPasswordCommand>().ConstructUsing(c => new SetPasswordCommand(c.Username, c.NewPassword, c.ConfirmPassword));
            _ = this.CreateMap<RemoveAccountViewModel, RemoveAccountCommand>().ConstructUsing(c => new RemoveAccountCommand(c.Username));
            _ = this.CreateMap<SaveUserClaimViewModel, SaveUserClaimCommand>().ConstructUsing(c => new SaveUserClaimCommand(c.Username, c.Type, c.Value));
            _ = this.CreateMap<RemoveUserClaimViewModel, RemoveUserClaimCommand>().ConstructUsing(c => new RemoveUserClaimCommand(c.Username, c.Type, c.Value));
            _ = this.CreateMap<RemoveUserRoleViewModel, RemoveUserRoleCommand>().ConstructUsing(c => new RemoveUserRoleCommand(c.Username, c.Role));
            _ = this.CreateMap<SaveUserRoleViewModel, SaveUserRoleCommand>().ConstructUsing(c => new SaveUserRoleCommand(c.Username, c.Role));
            _ = this.CreateMap<RemoveUserLoginViewModel, RemoveUserLoginCommand>().ConstructUsing(c => new RemoveUserLoginCommand(c.Username, c.LoginProvider, c.ProviderKey));
            _ = this.CreateMap<AdminChangePasswordViewodel, AdminChangePasswordCommand>().ConstructUsing(c => new AdminChangePasswordCommand(c.Password, c.ConfirmPassword, c.Username));
            _ = this.CreateMap<IDomainUser, string>().ConstructUsing(s => s.UserName);
            /*
             * Domain to view model
             */
            _ = this.CreateMap<IDomainUser, UserViewModel>(MemberList.Destination);
            _ = this.CreateMap<IDomainUser, UserListViewModel>(MemberList.Destination);
            _ = this.CreateMap<UserLogin, UserLoginViewModel>(MemberList.Destination);
            _ = this.CreateMap<StoredEvent, EventHistoryData>().ConstructUsing(a => new EventHistoryData(a.Message, a.AggregateId, a.Details, a.Timestamp.ToString(CultureInfo.InvariantCulture), a.User, a.MessageType, a.RemoteIpAddress, a.EventType)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            _ = this.CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));

        }
    }
}
