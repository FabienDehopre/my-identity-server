namespace Dehopre.Sso.EntityFramework.Repository.Interfaces
{
    using Dehopre.EntityFrameworkCore.Interfaces;
    using Dehopre.Sso.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public interface ISsoContext : IDehopreEntityFrameworkStore
    {
        /// <summary>
        /// Template of an e-mail to easy access
        /// </summary>
        DbSet<Template> Templates { get; set; }

        /// <summary>
        /// This a implemented template to send communication sot users
        /// </summary>
        DbSet<Email> Emails { get; set; }

        /// <summary>
        /// SSO use this database to store his configs. Such version, Cloud credentials, e-mail credentials.
        /// </summary>
        DbSet<GlobalConfigurationSettings> GlobalConfigurationSettings { get; set; }
    }
}
