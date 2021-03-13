namespace Dehopre.Sso.Application.CloudServices.Email
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.Extensions;
    using Dehopre.Sso.Domain.Interfaces;
    using Dehopre.Sso.Domain.ViewModels.User;
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Logging;
    using MimeKit;
    using MimeKit.Text;

    public class GeneralSmtpService : IEmailService
    {
        private readonly ILogger<GeneralSmtpService> logger;
        private readonly IGlobalConfigurationSettingsService globalConfigurationSettingsService;

        public GeneralSmtpService(ILogger<GeneralSmtpService> logger, IGlobalConfigurationSettingsService globalConfigurationSettingsService)
        {
            this.logger = logger;
            this.globalConfigurationSettingsService = globalConfigurationSettingsService;
        }

        public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            var emailConfiguration = await this.globalConfigurationSettingsService.GetPrivateSettings(cancellationToken);

            if (emailConfiguration == null || !emailConfiguration.SendEmail)
            {
                return;
            }

            var mimeMessage = new MimeMessage();
            mimeMessage.To.Add(MailboxAddress.Parse(message.Email));
            mimeMessage.From.Add(new MailboxAddress(message.Sender.Name, message.Sender.Address));

            if (message.Bcc != null && message.Bcc.IsValid())
            {
                mimeMessage.To.AddRange(message.Bcc.Recipients.ToMailboxAddress());
            }

            mimeMessage.Subject = message.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            mimeMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            this.logger.LogInformation($"Sending e-mail to {message.Email}");
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(emailConfiguration.Smtp.Server, emailConfiguration.Smtp.Port, emailConfiguration.Smtp.UseSsl, cancellationToken);
                _ = client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(emailConfiguration.Smtp.Username, emailConfiguration.Smtp.Password, cancellationToken);
                await client.SendAsync(mimeMessage, cancellationToken);
                client.Disconnect(true, cancellationToken);
            }

            this.logger.LogInformation($"E-mail to {message.Email}: sent!");
        }
    }
}
