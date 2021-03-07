namespace Dehopre.Domain.Core.Bus
{
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Commands;
    using Dehopre.Domain.Core.Events;

    public interface IMediatorHandler
    {
        Task<bool> SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
