namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    public class RemoveAccountViewModel
    {
        public RemoveAccountViewModel(string username) => this.Username = username;

        public string Username { get; set; }

    }
}
