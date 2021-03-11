namespace Dehopre.Sso.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Domain.Core.Models;
    using Dehopre.Domain.Core.Util;
    using Dehopre.Sso.Domain.Commands.Email;
    using Dehopre.Sso.Domain.Commands.User;
    using Dehopre.Sso.Domain.ViewModels;
    using Dehopre.Sso.Domain.ViewModels.User;

    public class Email : Entity
    {
        public Email() { }

        public Email(string content, string subject, Sender sender, EmailType type, BlindCarbonCopy bcc = null)
        {
            this.Id = Guid.NewGuid();
            this.Content = content;
            this.Sender = sender;
            this.Subject = subject;
            this.Type = type;
            this.Bcc = bcc;
        }

        public EmailType Type { get; private set; }
        public string Content { get; private set; }
        public string Subject { get; private set; }
        public Sender Sender { get; private set; }
        public BlindCarbonCopy Bcc { get; private set; }
        public string UserName { get; protected set; }
        public DateTime Updated { get; private set; } = DateTime.UtcNow;

        public void Update(SaveEmailCommand request)
        {
            this.Subject = request.Subject;
            this.Content = request.Content;

            if (request.Bcc != null && request.Bcc.IsValid())
            {
                this.Bcc = request.Bcc;
            }

            if (request.Sender != null && request.Sender.IsValid())
            {
                this.Sender = request.Sender;
            }

            this.UserName = request.Username;
            this.Updated = DateTime.UtcNow;
        }

        public EmailMessage GetMessage(IDomainUser user, AccountResult created, UserCommand command, IEnumerable<Claim> claims) => new(
                user.Email,
                this.Bcc,
                this.GetFormatedContent(this.Subject, user, created, command, claims),
                this.GetFormatedContent(this.Content, user, created, command, claims),
                this.Sender);

        private string GetFormatedContent(string content, IDomainUser user, AccountResult created, UserCommand command, IEnumerable<Claim> claims)
        {
            if (content is null)
            {
                return string.Empty;
            }

            return content
                .Replace("{{picture}}", claims.ValueOf(JwtClaimTypes.Picture))
                .Replace("{{name}}", claims.ValueOf(JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, JwtClaimTypes.GivenName))
                .Replace("{{username}}", user.UserName)
                .Replace("{{code}}", created.Code)
                .Replace("{{url}}", created.Url)
                .Replace("{{provider}}", command.Provider)
                .Replace("{{phoneNumber}}", user.PhoneNumber)
                .Replace("{{email}}", user.Email);
        }
    }
}
