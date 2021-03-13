namespace Dehopre.Sso.Application.ViewModels.RoleViewModels
{
    public class RemoveUserFromRoleViewModel
    {
        public RemoveUserFromRoleViewModel(string role, string username)
        {
            this.Role = role;
            this.Username = username;
        }

        public string Role { get; set; }
        public string Username { get; set; }
    }
}
