namespace Dehopre.Sso.Domain.ViewModels.Settings
{
    using System.ComponentModel;

    public enum LoginStrategyType
    {
        [Description("ASP.NET Identity")]
        Identity = 1,
        [Description("LDAP")]
        Ldap = 2,
    }
}
