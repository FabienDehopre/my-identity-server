namespace Dehopre.Sso.Domain.ViewModels.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PrivateSettings
    {
        public PrivateSettings(Settings settings)
        {
            if (!settings.Any())
            {
                return;
            }

            this.Smtp = new Smtp(settings["Smtp:Server"], settings["Smtp:Port"], settings["Smtp:UseSsl"], settings["Smtp:Password"], settings["Smtp:Username"]);
            this.Storage = new StorageSettings(
                                        settings["Storage:Username"],
                                        settings["Storage:Password"],
                                        settings["Storage:Service"],
                                        settings["Storage:StorageName"],
                                        settings["Storage:PhysicalPath"],
                                        settings["Storage:VirtualPath"],
                                        settings["Storage:BasePath"],
                                        settings["Storage:Region"]);

            this.Recaptcha = new RecaptchaSettings(settings["Recaptcha:SiteKey"], settings["Recaptcha:PrivateKey"]);
            this.LdapSettings = new LdapSettings(
                settings["Ldap:DomainName"],
                settings["Ldap:DistinguishedName"],
                settings["Ldap:Attributes"],
                settings["Ldap:AuthType"],
                settings["Ldap:SearchScope"],
                settings["Ldap:PortNumber"],
                settings["Ldap:FullyQualifiedDomainName"],
                settings["Ldap:ConnectionLess"],
                settings["Ldap:Address"]);

            if (bool.TryParse(settings["SendEmail"], out var sendEmail))
            {
                this.SendEmail = sendEmail;
            }

            if (bool.TryParse(settings["UseStorage"], out var useStorage))
            {
                this.UseStorage = useStorage;
            }

            if (bool.TryParse(settings["UseRecaptcha"], out var useRecaptcha))
            {
                this.UseRecaptcha = useRecaptcha;
                ;
            }

            if (settings.ContainsKey("LoginStrategy"))
            {
                this.LoginStrategy = Enum.Parse<LoginStrategyType>(settings["LoginStrategy"]);
            }
        }

        public LdapSettings LdapSettings { get; set; }
        public LoginStrategyType LoginStrategy { get; set; }
        public bool UseRecaptcha { get; set; }
        public bool UseStorage { get; }
        public bool SendEmail { get; }

        public Smtp Smtp { get; }
        public StorageSettings Storage { get; }
        public RecaptchaSettings Recaptcha { get; }
    }
}
