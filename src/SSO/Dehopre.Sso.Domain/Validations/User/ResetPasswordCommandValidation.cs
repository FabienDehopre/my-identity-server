namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;
    using FluentValidation;

    public class ResetPasswordCommandValidation : UserValidation<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidation()
        {
            this.ValidateEmail();
            this.ValidatePassword();
            this.ValidateCode();
        }

        protected new void ValidateCode() => this.RuleFor(c => c.Code).NotEmpty();

    }
}
