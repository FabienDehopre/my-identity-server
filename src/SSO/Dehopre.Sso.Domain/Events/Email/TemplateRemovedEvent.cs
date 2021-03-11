namespace Dehopre.Sso.Domain.Events.Email
{
    using System;
    using Dehopre.Domain.Core.Events;

    public class TemplateRemovedEvent : Event
    {
        public TemplateRemovedEvent(Guid templateId)
        {
            this.AggregateId = templateId.ToString();
            this.Message = "Template";
        }
    }
}
