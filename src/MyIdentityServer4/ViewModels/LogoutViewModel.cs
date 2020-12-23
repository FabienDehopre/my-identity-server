namespace MyIdentityServer4.ViewModels
{
    using MyIdentityServer4.InputModel;

    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
