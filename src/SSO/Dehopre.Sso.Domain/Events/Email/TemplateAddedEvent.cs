namespace Dehopre.Sso.Domain.Events.Email
{
    using Dehopre.Domain.Core.Events;
    using Dehopre.Sso.Domain.Models;

    public class TemplateAddedEvent : Event
    {
        public Template Template { get; }

        public TemplateAddedEvent(Template template)
            : base(EventTypes.Success)
        {
            this.Template = template;
            this.AggregateId = template.Id.ToString();
            this.Message = "Template Added";
        }
    }
}
