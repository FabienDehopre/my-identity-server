namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class SaveUserClaimViewModel
    {
        [Required]
        public string Value { get; set; }

        [Required]
        public string Type { get; set; }

        [JsonIgnore]
        public string Username { get; set; }
    }
}
