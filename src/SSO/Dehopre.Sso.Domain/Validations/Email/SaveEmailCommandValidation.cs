namespace Dehopre.Sso.Domain.Validations.Email
{
    using Dehopre.Sso.Domain.Commands.Email;

    public class SaveEmailCommandValidation : EmailValidation<SaveEmailCommand>
    {
        public SaveEmailCommandValidation()
        {
            this.ValidateSubject();
            this.ValidateSubject();
            this.ValidateSendAddress();
            this.ValidateSendName();
        }
    }
}
