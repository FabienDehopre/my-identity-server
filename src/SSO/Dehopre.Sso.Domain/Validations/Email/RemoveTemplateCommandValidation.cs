namespace Dehopre.Sso.Domain.Validations.Email
{
    using Dehopre.Sso.Domain.Commands.Email;

    public class RemoveTemplateCommandValidation : TemplateValidation<RemoveTemplateCommand>
    {
        public RemoveTemplateCommandValidation() => this.ValidateName();
    }
}
