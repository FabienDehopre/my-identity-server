namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveUserRoleViewModel
    {
        public RemoveUserRoleViewModel(string username, string role)
        {
            this.Username = username;
            this.Role = role;
        }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Username { get; set; }

    }
}
