namespace Dehopre.Sso.Domain.Validations.Email
{
    using Dehopre.Sso.Domain.Commands.Email;

    public class UpdateTemplateCommandValidation : TemplateValidation<UpdateTemplateCommand>
    {
        public UpdateTemplateCommandValidation()
        {
            this.ValidateOldName();
            this.ValidateName();
            this.ValidateSubject();
            this.ValidateContent();
        }
    }
}
