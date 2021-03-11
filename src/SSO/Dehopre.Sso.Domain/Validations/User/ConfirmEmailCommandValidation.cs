namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class ConfirmEmailCommandValidation : UserValidation<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidation()
        {
            this.ValidateEmail();
            this.ValidateCode();
        }
    }
}
