namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveUserClaimViewModel
    {
        public RemoveUserClaimViewModel(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
        public RemoveUserClaimViewModel(string username, string type, string value)
        {
            this.Username = username;
            this.Type = type;
            this.Value = value;
        }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Username { get; set; }

        public string Value { get; set; }
    }
}
