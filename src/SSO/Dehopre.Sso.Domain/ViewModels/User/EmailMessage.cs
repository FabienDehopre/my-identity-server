namespace Dehopre.Sso.Domain.ViewModels.User
{
    using Dehopre.Sso.Domain.Models;

    public class EmailMessage
    {
        public string Email { get; }
        public BlindCarbonCopy Bcc { get; }
        public string Subject { get; }
        public string Content { get; }
        public Sender Sender { get; }

        public EmailMessage(string email, BlindCarbonCopy bcc, string subject, string content, Sender sender)
        {
            this.Email = email;
            this.Bcc = bcc;
            this.Subject = subject;
            this.Content = content;
            this.Sender = sender;
        }
    }
}
