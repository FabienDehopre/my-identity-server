namespace Dehopre.Sso.Domain.Commands.Email
{
    using Dehopre.Sso.Domain.Validations.Email;

    public class RemoveTemplateCommand : TemplateCommand
    {

        public RemoveTemplateCommand(string name) => this.Name = name.Trim();

        public override bool IsValid()
        {
            this.ValidationResult = new RemoveTemplateCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }

    }
}
