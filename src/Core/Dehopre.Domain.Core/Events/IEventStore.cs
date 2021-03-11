namespace Dehopre.Domain.Core.Events
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEventStore
    {
        Task Save<T>(T theEvent, CancellationToken cancellationToken = default) where T : Event;
    }
}
