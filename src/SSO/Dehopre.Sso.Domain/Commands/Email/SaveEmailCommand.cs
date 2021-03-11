namespace Dehopre.Sso.Domain.Commands.Email
{
    using Dehopre.Sso.Domain.Models;
    using Dehopre.Sso.Domain.Validations.Email;

    public class SaveEmailCommand : EmailCommand
    {
        public SaveEmailCommand(string content, Sender sender, string subject, EmailType type, BlindCarbonCopy bcc, string username)
        {
            this.Sender = sender;
            this.Content = content;
            this.Subject = subject;
            this.Type = type;
            this.Bcc = bcc;
            this.Username = username;
        }

        public override bool IsValid()
        {
            this.ValidationResult = new SaveEmailCommandValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }

        public Email ToModel() => new(this.Content, this.Subject, this.Sender, this.Type, this.Bcc);
    }
}
