namespace Dehopre.Sso.Domain.Events.Email
{
    using Dehopre.Domain.Core.Events;
    using Dehopre.Sso.Domain.Models;

    public class EmailSavedEvent : Event
    {
        public Email Email { get; }

        public EmailSavedEvent(Email email)
            : base(EventTypes.Success)
        {
            this.Email = email;
            this.AggregateId = email.Type.ToString();
        }

    }
}
