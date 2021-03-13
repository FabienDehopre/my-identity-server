namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class ConfirmEmailViewModel
    {
        [JsonIgnore]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

    }
}
