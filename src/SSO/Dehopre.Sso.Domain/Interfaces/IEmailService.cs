namespace Dehopre.Sso.Domain.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Sso.Domain.ViewModels.User;

    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }
}
