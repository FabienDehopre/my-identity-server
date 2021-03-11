namespace Dehopre.Sso.Domain.Models
{
    using Dehopre.Sso.Domain.Interfaces;

    public class Role : IRole
    {
        public Role(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
