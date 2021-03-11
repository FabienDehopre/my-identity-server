namespace Dehopre.Sso.Domain.Validations.UserManagement
{
    using Dehopre.Sso.Domain.Commands.UserManagement;
    using FluentValidation;

    public abstract class ProfileValidation<T> : AbstractValidator<T> where T : ProfileCommand
    {
        protected void ValidateUsername() => this.RuleFor(c => c.Username).NotEmpty().WithMessage("Invalid user");

        protected void ValidatePicture() => this.RuleFor(c => c.Picture).NotEmpty().WithMessage("Please ensure you have entered the picture");
    }
}
