namespace Dehopre.Sso.Application.EventSourcedNormalizers
{
    using Dehopre.Domain.Core.Events;
    using Dehopre.Domain.Core.Util;

    public class EventHistoryData
    {
        public EventHistoryData(string action, string aggregate, EventDetails details, string when, string who, string category, string ip, EventTypes eventType)
        {
            this.Action = action;
            this.When = when;
            this.Who = who;
            this.Aggregate = aggregate;
            this.Category = category;
            this.Ip = ip;
            if ((int)eventType > 0)
            {
                this.EventType = eventType.ToString().AddSpacesToSentence();
            }

            this.Details = details?.Metadata;
        }

        public string Category { get; }
        public string Ip { get; }
        public string EventType { get; }
        public string Action { get; }
        public string Aggregate { get; private set; }
        public string When { get; }
        public string Who { get; private set; }

        public string Details { get; private set; }

        public void MarkAsSensitiveData()
        {
            if (!this.Who.IsNullOrEmpty() && this.Who.Contains('@'))
            {
                this.Who = this.Who.TruncateEmail();
            }
            else
            {
                this.Who = this.Who?.TruncateSensitiveInformation();
            }

            this.Details = this.Details?.TruncateSensitiveInformation();
            this.Aggregate = this.Aggregate?.TruncateSensitiveInformation();
        }
    }
}
