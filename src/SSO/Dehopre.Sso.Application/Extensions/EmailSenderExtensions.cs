namespace Dehopre.Sso.Application.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using MimeKit;

    public static class EmailSenderExtensions
    {
        public static IEnumerable<MailboxAddress> ToMailboxAddress(this string[] recipients) => recipients.Select(s =>
        {
            if (MailboxAddress.TryParse(s, out var ma))
            {
                return ma;
            }

            return null;
        }).Where(ma => ma is not null);
    }
}
