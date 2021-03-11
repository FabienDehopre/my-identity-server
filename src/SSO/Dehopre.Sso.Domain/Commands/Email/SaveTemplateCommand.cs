namespace Dehopre.Sso.Domain.Commands.Email
{
    using Dehopre.Sso.Domain.Models;
    using Dehopre.Sso.Domain.Validations.Email;

    public class SaveTemplateCommand : TemplateCommand
    {

        public SaveTemplateCommand(string subject, string content, string name, string userName)
        {
            this.UserName = userName;
            this.Subject = subject;
            this.Content = content;
            this.Name = name.Trim();
        }

        public override bool IsValid()
        {
            this.ValidationResult = new AddTemplateCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }

        public Template ToModel() => new(this.Content, this.Subject, this.Name, this.UserName);
    }
}
