namespace Dehopre.Sso.Domain.ViewModels.Settings
{
    using System.Collections.Generic;
    using Dehopre.Sso.Domain.Models;

    public class Settings : Dictionary<string, string>
    {
        public new string this[string key] => this.ContainsKey(key) ? base[key] : null;

        public static readonly string[] AvailableSettings = { "Smtp:Server", "Smtp:Password", "Smtp:Username", "Smtp:Port", "Smtp:UseSsl", "SendEmail" };

        public Settings(IEnumerable<GlobalConfigurationSettings> configuration)
        {
            foreach (var globalConfigurationSettingse in configuration)
            {
                _ = this.TryAdd(globalConfigurationSettingse.Key, globalConfigurationSettingse.Value);
            }
        }
    }
}
