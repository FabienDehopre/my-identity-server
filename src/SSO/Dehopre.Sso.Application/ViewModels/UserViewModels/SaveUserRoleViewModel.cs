namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class SaveUserRoleViewModel
    {
        [Required]
        public string Role { get; set; }

        [JsonIgnore]
        public string Username { get; set; }
    }
}
