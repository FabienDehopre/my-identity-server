namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveUserLoginViewModel
    {
        public RemoveUserLoginViewModel(string username, string loginProvider, string providerKey)
        {
            this.Username = username;
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
        }

        [Required]
        public string Username { get; set; }

        [Required]
        public string LoginProvider { get; set; }

        [Required]
        public string ProviderKey { get; set; }
    }
}
