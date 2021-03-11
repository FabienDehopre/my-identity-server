namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Interfaces;
    using Dehopre.Sso.Domain.Models;

    public interface IEmailRepository : IRepository<Email>
    {
        Task<Email> GetByType(EmailType type);
    }
}
