namespace Dehopre.Sso.Domain.ViewModels.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Util;

    public class StorageSettings
    {
        public string Username { get; }
        public string Password { get; }
        public string VirtualPath { get; }
        public StorageProviderService Provider { get; }
        public string StorageName { get; }
        public string BasePath { get; }
        public string Region { get; }
        public string PhysicalPath { get; }

        /// <summary>
        /// Repository settings
        /// </summary>
        /// <param name="username">Service username</param>
        /// <param name="password">Service key</param>
        /// <param name="storageService">S3 / Azure / Local</param>
        /// <param name="storageName">The service name of storage (AWS S3 name)</param>
        /// <param name="virtualPath">For local storage will concate to end of path: https://sso.jpproject.net/virtual-path/ </param>
        /// <param name="basePath">The base path to return in local storage: https://sso.jpproject.net/ </param>
        /// <param name="region">AWS Region</param>
        public StorageSettings(string username, string password, string storageService, string storageName, string physicalPath, string virtualPath, string basePath, string region)
        {
            this.Username = username;
            this.Password = password;
            this.VirtualPath = virtualPath;
            this.BasePath = basePath;
            this.Region = region;
            this.StorageName = storageName;
            this.PhysicalPath = physicalPath;

            if (storageService.IsPresent())
            {
                if (Enum.TryParse<StorageProviderService>(storageService, true, out var result))
                {
                    this.Provider = result;
                }
            }
        }
    }
}
