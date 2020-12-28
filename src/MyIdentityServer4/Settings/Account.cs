namespace MyIdentityServer4.Settings
{
    using System;

    public class Account
    {
        public bool AllowLocalLogin { get; set; }
        public bool AllowRememberLogin { get; set; }
        public TimeSpan RememberMeLoginDuration { get; set; }
        public bool ShowLogoutPrompt { get; set; }
        public bool AutomaticRedirectAfterSignOut { get; set; }
    }
}
