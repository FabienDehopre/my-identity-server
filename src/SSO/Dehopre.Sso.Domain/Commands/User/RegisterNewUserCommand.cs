namespace Dehopre.Sso.Domain.Commands.User
{
    using System;
    using Dehopre.Sso.Domain.Validations.User;

    public class RegisterNewUserCommand : UserCommand
    {
        public RegisterNewUserCommand(string username, string email, string name, string phoneNumber, string password, string confirmPassword, DateTime? birthdate, string ssn, bool shouldConfirmEmail = false)
        {
            this.Birthdate = birthdate;
            this.Username = username;
            this.Email = email;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
            this.SocialNumber = ssn;
            this.EmailConfirmed = !shouldConfirmEmail;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new RegisterNewUserCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }

    }
}
