namespace Dehopre.Sso.Domain.Commands.Email
{
    using Dehopre.Sso.Domain.Validations.Email;

    public class UpdateTemplateCommand : TemplateCommand
    {
        public UpdateTemplateCommand(string oldname, string subject, string content, string name, string userName)
        {
            this.OldName = oldname;
            this.UserName = userName;
            this.Subject = subject;
            this.Content = content;
            this.Name = name.Trim();
        }

        public override bool IsValid()
        {
            this.ValidationResult = new UpdateTemplateCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }

    }
}
