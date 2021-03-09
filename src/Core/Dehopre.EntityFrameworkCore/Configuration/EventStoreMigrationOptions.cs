namespace Dehopre.EntityFrameworkCore.Configuration
{
    public class EventStoreMigrationOptions
    {
        public bool Migrate { get; set; }
        public static EventStoreMigrationOptions Get() => new EventStoreMigrationOptions();
        public EventStoreMigrationOptions ShouldMigrate(bool migrate = true)
        {
            this.Migrate = migrate;
            return this;
        }
    }
}
