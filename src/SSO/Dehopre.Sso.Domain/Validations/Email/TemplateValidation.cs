namespace Dehopre.Sso.Domain.Validations.Email
{
    using System;
    using Dehopre.Sso.Domain.Commands.Email;
    using FluentValidation;

    public abstract class TemplateValidation<T> : AbstractValidator<T> where T : TemplateCommand
    {

        protected void ValidateContent() => this.RuleFor(c => c.Content).NotEmpty();
        protected void ValidateSubject() => this.RuleFor(c => c.Subject).NotEmpty();
        protected void ValidateName() => this.RuleFor(c => c.Name).NotEmpty().Must(a => !a.Contains(" ") && Uri.IsWellFormedUriString(a, UriKind.RelativeOrAbsolute));
        protected void ValidateOldName() => this.RuleFor(c => c.OldName).NotEmpty();
    }
}
