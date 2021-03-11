namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using FluentValidation;

    public abstract class UserManagementValidation<T> : AbstractValidator<T> where T : UserManagementCommand
    {
        protected void ValidateName() => this.RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the name")
                .Length(2, 150).WithMessage("The name must have between 2 and 150 characters");


        protected void ValidateEmail() => this.RuleFor(c => c.Email).NotEmpty().EmailAddress();


        protected void ValidateUsername() => this.RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Please ensure you have entered the Username")
                .Length(2, 50).WithMessage("The Username must have between 2 and 50 characters");

        protected void ValidatePassword() => this.RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please ensure you have entered the password")
                .Equal(c => c.ConfirmPassword).WithMessage("Password and Confirm password must be equal")
                .MinimumLength(8).WithMessage("Password minimun length must be 8 characters");

        protected void ValidateProvider() => this.RuleFor(c => c.Provider).NotEmpty();

        protected void ValidateProviderId() => this.RuleFor(c => c.ProviderId).NotEmpty();
    }
}
