namespace Dehopre.Sso.Domain.Events.UserManagement
{
    using Dehopre.Domain.Core.Events;
    using Dehopre.Sso.Domain.Commands.UserManagement;

    public class ProfileUpdatedEvent : Event
    {
        public UpdateProfileCommand Request { get; }

        public ProfileUpdatedEvent(string username, UpdateProfileCommand request)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Request = request;
        }
    }
}
