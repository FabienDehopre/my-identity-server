namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;
    using FluentValidation;

    public abstract class UserClaimValidation<T> : AbstractValidator<T> where T : UserClaimCommand
    {
        protected void ValidateUsername() => this.RuleFor(c => c.Username).NotEmpty().WithMessage("Username must be set");
        protected void ValidateValue() => this.RuleFor(c => c.Value).NotEmpty().WithMessage("Secret must be set");
        protected void ValidateKey() => this.RuleFor(c => c.Type).NotEmpty().WithMessage("Please ensure you have entered key");
        protected void ValidateClaims() => this.RuleFor(c => c.Claims).NotEmpty().WithMessage("Please provide at least one claim");
    }
}
