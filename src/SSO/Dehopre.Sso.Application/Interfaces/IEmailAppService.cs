namespace Dehopre.Sso.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Application.ViewModels.EmailViewModels;
    using Dehopre.Sso.Domain.Models;

    public interface IEmailAppService : IDisposable
    {
        Task<EmailViewModel> FindByType(EmailType type, CancellationToken cancellationToken = default);
        Task<bool> SaveEmail(EmailViewModel command, CancellationToken cancellationToken = default);
        Task<bool> SaveTemplate(TemplateViewModel command, CancellationToken cancellationToken = default);
        Task<IEnumerable<TemplateViewModel>> ListTemplates(CancellationToken cancellationToken = default);
        Task<bool> UpdateTemplate(TemplateViewModel model, CancellationToken cancellationToken = default);
        Task<TemplateViewModel> GetTemplate(string name, CancellationToken cancellationToken = default);
        Task<bool> RemoveTemplate(string name, CancellationToken cancellationToken = default);
    }
}
