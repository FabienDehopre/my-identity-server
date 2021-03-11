namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using FluentValidation;

    public abstract class PasswordCommandValidation<T> : AbstractValidator<T> where T : PasswordCommand
    {
        protected void ValidateOldPassword() => this.RuleFor(c => c.OldPassword)
                .NotEmpty().WithMessage("Please ensure you have entered the Old Password")
                .MinimumLength(8).WithMessage("Password minimun length must be 8 characters");

        protected void ValidatePassword() => this.RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please ensure you have entered the password")
                .Equal(c => c.ConfirmPassword).WithMessage("Password and Confirm password must be equal")
                .MinimumLength(8).WithMessage("Password minimun length must be 8 characters");

        protected void ValidateUsername() => this.RuleFor(c => c.Username).NotEmpty().WithMessage("Invalid user");
    }
}
