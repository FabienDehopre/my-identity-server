using MyIdentityServer4.InputModel;

namespace MyIdentityServer4.ViewModels
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
