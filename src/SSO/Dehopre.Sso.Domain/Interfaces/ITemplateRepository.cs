namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Sso.Domain.Models;

    public interface ITemplateRepository : IRepository<Template>
    {
        Task<bool> Exist(string name);
        Task<Template> GetByName(string name);
        Task<List<Template>> All();
    }
}
