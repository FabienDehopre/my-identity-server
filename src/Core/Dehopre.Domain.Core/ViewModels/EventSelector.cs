namespace Dehopre.Domain.Core.ViewModels
{
    using Dehopre.Domain.Core.Util;

    public class EventSelector
    {
        public EventSelector() { }

        public EventSelector(AggregateType type, string aggregate)
        {
            this.AggregateType = type.ToString().AddSpacesToSentence();
            this.Aggregate = aggregate;
        }

        public string AggregateType { get; set; }
        public string Aggregate { get; set; }
    }
}
