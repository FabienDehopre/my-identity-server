namespace Dehopre.Sso.Domain.ViewModels.Settings
{
    using System;

    public class PublicSettings
    {
        public PublicSettings(Settings settings)
        {
            this.Logo = settings["Logo"];
            this.Version = new Version(settings["SSO:Version"]);
            this.UseRecaptcha = settings["UseRecaptcha"] == "true";
            this.RecaptchaSiteKey = settings["Recaptcha:SiteKey"];
        }

        public string RecaptchaSiteKey { get; set; }
        public bool UseRecaptcha { get; }
        public Version Version { get; }
        public string Logo { get; }
    }
}
