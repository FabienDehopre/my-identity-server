namespace Dehopre.Sso.Domain.Validations.Email
{
    using Dehopre.Sso.Domain.Commands.Email;

    public class AddTemplateCommandValidation : TemplateValidation<SaveTemplateCommand>
    {
        public AddTemplateCommandValidation()
        {
            this.ValidateName();
            this.ValidateContent();

        }
    }
}
