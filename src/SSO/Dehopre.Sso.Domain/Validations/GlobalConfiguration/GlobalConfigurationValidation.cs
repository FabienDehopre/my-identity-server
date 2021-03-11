namespace Dehopre.Sso.Domain.Validations.GlobalConfiguration
{
    using Dehopre.Sso.Domain.Commands.GlobalConfiguration;
    using FluentValidation;

    public abstract class GlobalConfigurationValidation<T> : AbstractValidator<T> where T : GlobalConfigurationCommand
    {
        protected void ValidateKey() => this.RuleFor(r => r.Key).NotEmpty();
        protected void ValidateValue() => this.RuleFor(r => r.Key).NotEmpty();
    }
}
