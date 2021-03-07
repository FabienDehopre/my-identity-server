namespace Dehopre.Domain.Core.Commands
{
    using System;
    using Dehopre.Domain.Core.Events;
    using FluentValidation.Results;
    using MediatR;

    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command() => this.Timestamp = DateTime.Now;

        public abstract bool IsValid();
    }
}
