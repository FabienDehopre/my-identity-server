namespace Dehopre.Sso.Domain.Events.UserManagement
{
    using Dehopre.Domain.Core.Events;

    public class ProfilePictureUpdatedEvent : Event
    {
        public string Picture { get; }

        public ProfilePictureUpdatedEvent(string username, string picture)
            : base(EventTypes.Success)
        {
            this.AggregateId = username;
            this.Picture = picture;
        }
    }
}
