namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dehopre.Sso.Domain.Models;

    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRoles();
        Task<bool> Remove(string name);
        Task<Role> Details(string name);
        Task<bool> Save(string name);
        Task<bool> Update(string name, string oldName);
    }
}
