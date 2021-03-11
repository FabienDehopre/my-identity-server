namespace Dehopre.Sso.Domain.Commands.User
{
    using System;
    using Dehopre.Sso.Domain.Validations.User;

    public class RegisterNewUserWithProviderCommand : UserCommand
    {
        public RegisterNewUserWithProviderCommand(string username, string email, string name, string phoneNumber, string password, string confirmPassword, string picture, string provider, string providerId, DateTime? birthdate, string socialNumber)
        {
            this.Birthdate = birthdate;
            this.SocialNumber = socialNumber;
            this.Picture = picture;
            this.Provider = provider;
            this.ProviderId = providerId;
            this.Username = username;
            this.Email = email;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }


        public override bool IsValid()
        {
            this.ValidationResult = new RegisterNewUserWithProviderCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
