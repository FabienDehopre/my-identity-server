namespace Dehopre.Sso.Application.Configuration
{
    using System.Threading.Tasks;
    using Dehopre.Domain.Core.Events;
    using Dehopre.Domain.Core.Interfaces;
    using IdentityServer4.Events;
    using IdentityServer4.Services;
    using EventTypes = Dehopre.Domain.Core.Events.EventTypes;
    using Is4Event = IdentityServer4.Events.Event;

    public class IdentityServerEventStore : IEventSink
    {
        private readonly IEventStoreRepository eventStoreRepository;
        private readonly ISystemUser user;

        public IdentityServerEventStore(IEventStoreRepository eventStoreRepository, ISystemUser user)
        {
            this.eventStoreRepository = eventStoreRepository;
            this.user = user;
        }

        public Task PersistAsync(Is4Event evt)
        {
            var es = new StoredEvent(
                evt.Category,
                (EventTypes)(int)evt.EventType,
                evt.Name,
                evt.LocalIpAddress,
                evt.RemoteIpAddress,
                    evt.ToString()
                ).ReplaceTimeStamp(evt.TimeStamp);

            if (this.user.IsAuthenticated())
            {
                _ = es.SetUser(this.user.Username);
            }

            switch (evt)
            {
                case ApiAuthenticationFailureEvent apiAuthenticationFailureEvent:
                    _ = es.SetAggregate(apiAuthenticationFailureEvent.ApiName);
                    break;
                case ApiAuthenticationSuccessEvent apiAuthenticationSuccessEvent:
                    _ = es.SetAggregate(apiAuthenticationSuccessEvent.ApiName);
                    break;
                case ClientAuthenticationFailureEvent clientAuthenticationFailureEvent:
                    _ = es.SetAggregate(clientAuthenticationFailureEvent.ClientId);
                    break;
                case ClientAuthenticationSuccessEvent clientAuthenticationSuccessEvent:
                    _ = es.SetAggregate(clientAuthenticationSuccessEvent.ClientId);
                    break;
                case ConsentDeniedEvent consentDeniedEvent:
                    _ = es.SetAggregate(consentDeniedEvent.ClientId);
                    break;
                case ConsentGrantedEvent consentGrantedEvent:
                    _ = es.SetAggregate(consentGrantedEvent.ClientId);
                    break;
                case DeviceAuthorizationFailureEvent deviceAuthorizationFailureEvent:
                    _ = es.SetAggregate(deviceAuthorizationFailureEvent.ClientId);
                    break;
                case DeviceAuthorizationSuccessEvent deviceAuthorizationSuccessEvent:
                    _ = es.SetAggregate(deviceAuthorizationSuccessEvent.ClientId);
                    break;
                case GrantsRevokedEvent grantsRevokedEvent:
                    _ = es.SetAggregate(grantsRevokedEvent.ClientId);
                    break;
                case InvalidClientConfigurationEvent invalidClientConfigurationEvent:
                    _ = es.SetAggregate(invalidClientConfigurationEvent.ClientId);
                    break;
                case TokenIntrospectionFailureEvent tokenIntrospectionFailureEvent:
                    _ = es.SetAggregate(tokenIntrospectionFailureEvent.ApiName);
                    break;
                case TokenIntrospectionSuccessEvent tokenIntrospectionSuccessEvent:
                    _ = es.SetAggregate(tokenIntrospectionSuccessEvent.ApiName);
                    break;
                case TokenIssuedFailureEvent tokenIssuedFailureEvent:
                    _ = es.SetAggregate(tokenIssuedFailureEvent.ClientId);
                    break;
                case TokenIssuedSuccessEvent tokenIssuedSuccessEvent:
                    _ = es.SetAggregate(tokenIssuedSuccessEvent.ClientId);
                    break;
                case TokenRevokedSuccessEvent tokenRevokedSuccessEvent:
                    _ = es.SetAggregate(tokenRevokedSuccessEvent.ClientId);
                    break;
                //case UnhandledExceptionEvent unhandledExceptionEvent:
                //    break;
                case UserLoginFailureEvent userLoginFailureEvent:
                    _ = es.SetUser(userLoginFailureEvent.Username).SetAggregate(userLoginFailureEvent.ClientId);
                    break;
                case UserLoginSuccessEvent userLoginSuccessEvent:
                    _ = es.SetUser(userLoginSuccessEvent.Username).SetAggregate(userLoginSuccessEvent.ClientId);
                    break;
                case UserLogoutSuccessEvent userLogoutSuccessEvent:
                    _ = es.SetAggregate(userLogoutSuccessEvent.SubjectId);
                    break;
            }

            return this.eventStoreRepository.Store(es);
        }

    }
}
