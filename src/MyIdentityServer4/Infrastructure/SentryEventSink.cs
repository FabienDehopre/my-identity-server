namespace MyIdentityServer4.Infrastructure
{
    using System.Threading.Tasks;
    using IdentityServer4.Events;
    using IdentityServer4.Services;
    using Serilog;
    using Serilog.Core;

    public class SentryEventSink : IEventSink
    {
        private readonly Logger log;

        public SentryEventSink() =>
            this.log = new LoggerConfiguration()
                .WriteTo.Sentry()
                .CreateLogger();

        public Task PersistAsync(Event evt)
        {
            if (evt.EventType == EventTypes.Success || evt.EventType == EventTypes.Information)
            {
                this.log.Information("{Name} ({Id}), Details {@details}", evt.Name, evt.Id, evt);
            }
            else
            {
                this.log.Error("{Name} ({Id}), Details {@details}", evt.Name, evt.Id, evt);
            }

            return Task.CompletedTask;
        }
    }
}
