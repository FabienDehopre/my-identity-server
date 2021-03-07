namespace Dehopre.Domain.Core.Events
{
    using MediatR;

    public abstract class Message : IRequest
    {
        public string MessageType { get; protected set; }
        public string AggregateId { get; protected set; }

        protected Message() => this.MessageType = this.GetType().Name;
    }
}
