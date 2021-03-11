namespace Dehopre.Sso.Domain.Commands.UserManagement
{
    using System;
    using Dehopre.Sso.Domain.Validations.UserManagement;

    public class UpdateProfileCommand : ProfileCommand
    {
        public UpdateProfileCommand(string username, string url, string bio, string company, string jobTitle, string name, string phoneNumber, string socialNumber, DateTime? birthdate)
        {
            this.SocialNumber = socialNumber;
            this.Birthdate = birthdate;
            this.Username = username;
            this.Url = url;
            this.Bio = bio;
            this.Company = company;
            this.JobTitle = jobTitle;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }


        public override bool IsValid()
        {
            this.ValidationResult = new UpdateProfileCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
