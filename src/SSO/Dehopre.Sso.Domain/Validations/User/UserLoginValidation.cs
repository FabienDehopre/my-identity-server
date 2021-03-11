namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;
    using FluentValidation;

    public abstract class UserLoginValidation<T> : AbstractValidator<T> where T : UserLoginCommand
    {
        protected void ValidateUsername() => this.RuleFor(c => c.Username).NotEmpty().WithMessage("Username must be set");
        protected void ValidateLoginProvider() => this.RuleFor(c => c.LoginProvider).NotEmpty().WithMessage("Login Provider must be set");
        protected void ValidateProviderKey() => this.RuleFor(c => c.ProviderKey).NotEmpty().WithMessage("Provider Key must be set");
    }
}
