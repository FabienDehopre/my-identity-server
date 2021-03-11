namespace Dehopre.Sso.Domain.Validations.Role
{
    using Dehopre.Sso.Domain.Commands.Role;
    using FluentValidation;

    public abstract class RoleValidation<T> : AbstractValidator<T> where T : RoleCommand
    {
        protected void ValidateName() => this.RuleFor(c => c.Name).NotEmpty().WithMessage("Name must be set");
        protected void ValidateNewName() => this.RuleFor(c => c.OldName).NotEmpty().NotEqual(c => c.Name).WithMessage("New name must be different from old one");
        protected void ValidateUsername() => this.RuleFor(c => c.Username).NotEmpty().WithMessage("Uername must be set");
    }
}
