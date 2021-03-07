namespace Dehopre.Domain.Core.Events
{
    using System.Threading.Tasks;

    public interface IEventStore
    {
        Task Save<T>(T theEvent) where T : Event;
    }
}
