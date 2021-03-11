namespace Dehopre.Sso.Domain.ViewModels.Settings
{
    public class LdapSettings
    {
        public LdapSettings() { }

        public LdapSettings(string domainName, string distinguishedName, string attributes, string authType, string searchScope, string portNumber, string fullyQualifiedDomainName, string connectionLess, string address)
        {
            this.DomainName = domainName;
            this.DistinguishedName = distinguishedName;
            this.Attributes = attributes;
            this.AuthType = authType;
            this.SearchScope = searchScope;
            this.Address = address;
            if (bool.TryParse(connectionLess, out var connectionParse))
            {
                this.ConnectionLess = connectionParse;
            }

            if (int.TryParse(portNumber, out var number))
            {
                this.PortNumber = number;
            }

            if (bool.TryParse(fullyQualifiedDomainName, out var fqdn))
            {
                this.FullyQualifiedDomainName = fqdn;
            }
        }

        public string DomainName { get; set; }
        public string DistinguishedName { get; set; }
        public string Attributes { get; set; }
        public string AuthType { get; set; }
        public string SearchScope { get; set; }
        public string Address { get; }
        public bool FullyQualifiedDomainName { get; }
        public bool ConnectionLess { get; }
        public int PortNumber { get; set; }
    }
}
