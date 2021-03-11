namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;
    using FluentValidation;

    public abstract class UserRoleValidation<T> : AbstractValidator<T> where T : UserRoleCommand
    {
        protected void ValidateUsername() => this.RuleFor(c => c.Username).NotEmpty().WithMessage("Username must be set");
        protected void ValidateRole() => this.RuleFor(c => c.Role).NotEmpty().WithMessage("Role must be set");
    }
}
