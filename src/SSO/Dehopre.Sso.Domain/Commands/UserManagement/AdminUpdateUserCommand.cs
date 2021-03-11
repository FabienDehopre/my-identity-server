namespace Dehopre.Sso.Domain.Commands.UserManagement
{
    using System;
    using Dehopre.Sso.Domain.Validations.UserManagement;

    public class AdminUpdateUserCommand : UserManagementCommand
    {
        public AdminUpdateUserCommand(string email, string userName, string name, string phoneNumber, bool emailConfirmed, bool phoneNumberConfirmed, bool twoFactorEnabled, DateTimeOffset? lockoutEnd, bool lockoutEnabled, int accessFailedCount, DateTime? birthdate, string ssn)
        {
            this.Birthdate = birthdate;
            this.SocialNumber = ssn;
            this.Email = email;
            this.Username = userName;
            this.EmailConfirmed = emailConfirmed;
            this.PhoneNumberConfirmed = phoneNumberConfirmed;
            this.TwoFactorEnabled = twoFactorEnabled;
            this.LockoutEnd = lockoutEnd;
            this.LockoutEnabled = lockoutEnabled;
            this.AccessFailedCount = accessFailedCount;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }


        public override bool IsValid()
        {
            this.ValidationResult = new UpdateUserCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
