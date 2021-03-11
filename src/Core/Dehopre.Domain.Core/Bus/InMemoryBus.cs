namespace Dehopre.Domain.Core.Bus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Commands;
    using Dehopre.Domain.Core.Events;
    using Dehopre.Domain.Core.Notifications;
    using MediatR;

    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator mediator;
        private readonly IEventStore eventStore;

        public InMemoryBus(IMediator mediator, IEventStore eventStore)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public async Task RaiseEvent<T>(T @event, CancellationToken cancellationToken = default) where T : Event
        {
            if (!@event.MessageType.Equals(nameof(DomainNotification)))
            {
                await this.eventStore.Save(@event, cancellationToken);
            }

            await this.mediator.Publish(@event, cancellationToken);
        }

        public Task<bool> SendCommand<T>(T command, CancellationToken cancellationToken = default) where T : Command => this.mediator.Send<bool>(command, cancellationToken);
    }
}
