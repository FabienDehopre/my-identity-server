namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;
    using FluentValidation;

    public abstract class UserValidation<T> : AbstractValidator<T> where T : UserCommand
    {
        protected void ValidateName() => this.RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Username")
                .Length(2, 150).WithMessage("The Username must have between 2 and 150 characters")
                .When(w => w.Name != null);


        protected void ValidateEmail() => this.RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress()
                .When(w => w.Email != null);

        protected void ValidateUsername() => this.RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Please ensure you have entered the Username")
                .Length(2, 50).WithMessage("The Username must have between 2 and 50 characters");

        protected void ValidateUsernameOrEmail() => this.RuleFor(c => c.EmailOrUsername)
                .NotEmpty().WithMessage("Please ensure you have entered the Username")
                .Length(2, 50).WithMessage("The Username must have between 2 and 50 characters");

        protected void ValidatePassword() => this.RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please ensure you have entered the password")
                .Equal(c => c.ConfirmPassword).WithMessage("Password and Confirm password must be equal")
                .MinimumLength(8).WithMessage("Password minimun length must be 8 characters");

        protected void ValidateProvider() => this.RuleFor(c => c.Provider)
                .NotEmpty()
                .When(c => c.CheckProvider);

        protected void ValidateProviderId() => this.RuleFor(c => c.ProviderId)
                .NotEmpty()
                .When(c => c.CheckProvider);

        protected void ValidateCode() => this.RuleFor(c => c.Code)
                .NotEmpty();

    }
}
