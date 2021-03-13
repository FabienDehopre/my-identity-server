namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordViewModel
    {
        public ForgotPasswordViewModel(string username) => this.UsernameOrEmail = username;

        [Required]
        public string UsernameOrEmail { get; set; }
    }
}
