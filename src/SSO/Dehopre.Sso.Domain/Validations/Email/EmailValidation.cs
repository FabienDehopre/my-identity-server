namespace Dehopre.Sso.Domain.Validations.Email
{
    using Dehopre.Sso.Domain.Commands.Email;
    using FluentValidation;

    public abstract class EmailValidation<T> : AbstractValidator<T> where T : EmailCommand
    {

        protected void ValidateBcc() => this.RuleFor(c => c.Bcc).NotEmpty();
        protected void ValidateSubject() => this.RuleFor(c => c.Subject).NotEmpty();
        protected void ValidateSendAddress() => this.RuleFor(c => c.Sender.Address).NotEmpty().EmailAddress();
        protected void ValidateSendName() => this.RuleFor(c => c.Sender.Name).NotEmpty();
        protected void ValidateDescription() => this.RuleFor(c => c.Description).NotEmpty();
        protected void ValidateUsername() => this.RuleFor(c => c.Username).NotEmpty();
    }
}
