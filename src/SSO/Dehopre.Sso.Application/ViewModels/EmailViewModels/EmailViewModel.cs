namespace Dehopre.Sso.Application.ViewModels.EmailViewModels
{
    using System.Text.Json.Serialization;
    using Dehopre.Sso.Domain.Models;

    public class EmailViewModel
    {
        public EmailType Type { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public BlindCarbonCopy Bcc { get; set; }

        [JsonIgnore]
        public string Username { get; set; }

        public Sender Sender { get; set; }
    }
}
